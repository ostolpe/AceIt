using AceIt.Data;
using AceIt.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Services;

public class SessionService(AppDbContext db, IAiService aiService) : ISessionService
{
    public async Task<SessionDto> StartSession()
    {
        var questions = await db.Questions
            .Select(x => new QuestionDto(x.Id, x.Text, x.Topic.ToString()))
            .ToListAsync();

        //ID 0 for now since we don't persist sessions yet
        return new SessionDto(0, questions);
    }

    public async Task<ResultDto> FinishSession(FinishSessionRequest request)
    {
        return await aiService.GradeSession(request);
    }
}