using AceIt;
using AceIt.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Aceit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> StartSession()
    {
        var result = await sessionService.StartSession();
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> FinishSession([FromBody] FinishSessionRequest request)
    {
        var result = await sessionService.FinishSession(request);
        return Ok(result);
    }
}