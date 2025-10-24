using Api.DTOs;
using Api.DTOs.Currency;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyController() : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetCurrencies", Summary = "Get all available currencies")]
    public ActionResult<ApiResponse<CurrencyListDto>> GetCurrencies() =>
        Ok(ApiResponse<CurrencyListDto>.Ok(
            new CurrencyListDto
            {
                Currencies = new List<CurrencyCode>()
                {
                    CurrencyCode.USD,
                    CurrencyCode.USD,
                    CurrencyCode.ZAR
                }
            }
        ));
}
