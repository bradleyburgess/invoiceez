using Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Extensions.ServiceExtensions;

public static class ModelStateExtensions
{
    public static Dictionary<string, string[]> ToErrorDictionary(this ModelStateDictionary modelState)
        => modelState.Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

    public static IServiceCollection ConfigureInvalidModelStateResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var response = ApiResponse<object>.Fail(
                    ApiResponseCode.ValidationError,
                    "Validation Errors",
                    context.ModelState.ToErrorDictionary()
                );
                return new BadRequestObjectResult(response);
            };
        });
        return services;
    }
}
