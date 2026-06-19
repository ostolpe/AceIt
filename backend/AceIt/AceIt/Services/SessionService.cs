using AceIt.Data;
using AceIt.DTOs;
using AceIt.Models;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Services;

public class SessionService(AppDbContext db, IAiService aiService) : ISessionService
{
    public async Task<SessionDto> StartSession(string userId)
    {
        var questions = await db.Questions
            .OrderBy(q => Guid.NewGuid())
            .Take(10)
            .ToListAsync();
        var session = new Session
        {
            UserId = userId,
            Questions = questions
        };

        db.Add(session);
        await db.SaveChangesAsync();

        var questionDtos = questions
            .Select(x => new QuestionDto(x.Id, x.Text, x.Topic.ToString())).ToList();

        return new SessionDto(session.Id, questionDtos);
    }

    public async Task<SessionSummaryDto> FinishSession(FinishSessionRequest request)
    {
        var session = await db.Sessions.FindAsync(request.SessionId)
            ?? throw new KeyNotFoundException($"Session {request.SessionId} not found.");

        var summary = await aiService.GradeSession(request);

        var answerLookup = request.Answers.ToDictionary(a => a.QuestionId, a => a.Answer);

        var results = summary.Results.Select(x => new QuestionResult
        {
            SessionId = request.SessionId,
            QuestionId = x.QuestionId,
            Score = x.Score,
            Feedback = x.Feedback,
            UserAnswer = answerLookup[x.QuestionId]
        });

        session.CompletedAt = DateTime.UtcNow;

        db.AddRange(results);
        await db.SaveChangesAsync();

        return summary;
    }
}
