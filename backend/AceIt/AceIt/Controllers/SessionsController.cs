using System.Security.Claims;
using AceIt.DTOs;
using AceIt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aceit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> StartSession()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User id claim missing from token.");

        var result = await sessionService.StartSession(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("submit")]
    public async Task<IActionResult> FinishSession([FromBody] FinishSessionRequest request)
    {
        var result = await sessionService.FinishSession(request);
        return Ok(result);
    }
}
