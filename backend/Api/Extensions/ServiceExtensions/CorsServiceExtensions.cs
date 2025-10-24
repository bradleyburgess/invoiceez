namespace Api.Extensions.ServiceExtensions;

public static class CorsServiceExtensions
{
    private static readonly string LocalHostPolicy = "_localHostPolicy";
    public static WebApplicationBuilder ConfigureCorsForDevelopment(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            System.Console.WriteLine("Development environment detected - enabling CORS for localhost:3000");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: LocalHostPolicy,
                poliicy =>
                {
                    poliicy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        return builder;
    }

    public static WebApplication ApplyCorsForDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseCors(LocalHostPolicy);
        return app;
    }
}
