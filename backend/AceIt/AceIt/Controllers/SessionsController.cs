using AceIt;
using AceIt.DTOs;
using AceIt.Services;
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
        // var result = await sessionService.FinishSession(request);
        // return Ok(result);
        var mockRes = new ResultDto(
        request.Answers.Select(a => new QuestionResult(
            a.QuestionId,
            new Random().Next(1, 10),
            "Mock feedback for testing purposes."
        )).ToList());

        return Ok(mockRes);
    }
}