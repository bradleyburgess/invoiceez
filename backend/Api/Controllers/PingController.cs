using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Ping() =>
        Ok(ApiResponse<string>.Ok(data: DateTime.UtcNow.ToString()));

}
