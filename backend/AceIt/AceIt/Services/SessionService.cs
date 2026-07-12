using AceIt.Data;
using AceIt.DTOs;
using AceIt.Exceptions;
using AceIt.Models;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Services;

public class SessionService(AppDbContext db, IAiService aiService) : ISessionService
{
    public async Task<SessionDto> StartSession(string userId, CancellationToken cancellationToken = default)
    {
        var questions = await db.Questions
            .OrderBy(q => Guid.NewGuid())
            .Take(10)
            .ToListAsync(cancellationToken);
        var session = new Session
        {
            UserId = userId,
            Questions = questions
        };

        db.Add(session);
        await db.SaveChangesAsync(cancellationToken);

        var questionDtos = questions
            .Select(x => new QuestionDto(x.Id, x.Text, x.Topic.ToString())).ToList();

        return new SessionDto(session.Id, questionDtos);
    }

    public async Task<SessionSummaryDto> FinishSession(string userId, FinishSessionRequest request, CancellationToken cancellationToken = default)
    {
        var session = await db.Sessions
            .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.UserId == userId, cancellationToken)
            ?? throw new NotFoundException("Could not find a matching session");

        var summary = await aiService.GradeSession(request, cancellationToken);

        var answerLookup = request.Answers.ToDictionary(a => a.QuestionId, a => a.Answer);

        var results = summary.Results
            .Where(x => answerLookup.ContainsKey(x.QuestionId))
            .Select(x => new QuestionResult
            {
                SessionId = request.SessionId,
                QuestionId = x.QuestionId,
                Score = x.Score,
                Feedback = x.Feedback,
                UserAnswer = answerLookup[x.QuestionId]
            });

        session.CompletedAt = DateTime.UtcNow;

        db.AddRange(results);
        await db.SaveChangesAsync(cancellationToken);

        return summary;
    }
}
