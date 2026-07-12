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
    public async Task<IActionResult> StartSession(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User id claim missing from token.");

        var result = await sessionService.StartSession(userId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("submit")]
    public async Task<IActionResult> FinishSession([FromBody] FinishSessionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User id claim missing from token.");

        var result = await sessionService.FinishSession(userId, request, cancellationToken);
        return Ok(result);
    }
}
