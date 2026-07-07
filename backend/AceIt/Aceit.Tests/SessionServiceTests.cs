using AceIt.Data;
using AceIt.DTOs;
using AceIt.Models;
using AceIt.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Tests;

/// <summary>
/// Tests for <see cref="SessionService"/> backed by a real (in-memory) SQLite
/// database so relational constraints — including the session-ownership filter —
/// behave like production.
/// </summary>
public class SessionServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;

    public SessionServiceTests()
    {
        // A SQLite in-memory database lives only as long as its connection is open,
        // so we hold the connection for the lifetime of the test.
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    /// <summary>
    /// Grading stub that records whether it was invoked. By default it grades
    /// every submitted answer; pass <paramref name="grader"/> to return a custom
    /// response (e.g. an unexpected question id).
    /// </summary>
    private sealed class FakeAiService(Func<FinishSessionRequest, SessionSummaryDto>? grader = null) : IAiService
    {
        public bool WasCalled { get; private set; }

        public Task<SessionSummaryDto> GradeSession(FinishSessionRequest request)
        {
            WasCalled = true;
            var summary = grader?.Invoke(request) ?? DefaultGrade(request);
            return Task.FromResult(summary);
        }

        private static SessionSummaryDto DefaultGrade(FinishSessionRequest request) =>
            new(request.Answers
                .Select(a => new QuestionResultDto(a.QuestionId, 8, "Solid answer."))
                .ToList());
    }

    private async Task<(Session session, Question question)> SeedSessionForAsync(string userId)
    {
        var user = new User { Id = userId, UserName = $"{userId}@test.dev", Email = $"{userId}@test.dev" };
        var question = new Question { Text = "Explain async/await.", Topic = Topic.CSharp };
        var session = new Session { UserId = userId, Questions = [question] };

        _db.Users.Add(user);
        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();

        return (session, question);
    }

    [Fact]
    public async Task FinishSession_WhenCallerOwnsSession_GradesAndPersistsResults()
    {
        var (session, question) = await SeedSessionForAsync("owner");
        var ai = new FakeAiService();
        var sut = new SessionService(_db, ai);
        var request = new FinishSessionRequest(session.Id, [new AnswerDto(question.Id, "My answer.")]);

        var summary = await sut.FinishSession("owner", request);

        Assert.True(ai.WasCalled);
        Assert.Single(summary.Results);

        var saved = await _db.QuestionResults.SingleAsync();
        Assert.Equal(question.Id, saved.QuestionId);
        Assert.Equal("My answer.", saved.UserAnswer);

        var completed = await _db.Sessions.AsNoTracking().SingleAsync(s => s.Id == session.Id);
        Assert.NotNull(completed.CompletedAt);
    }

    // Regression test for the IDOR fix (BACKLOG P0 #1): a caller must not be able
    // to grade or mutate a session they don't own.
    [Fact]
    public async Task FinishSession_WhenCallerDoesNotOwnSession_ThrowsAndPersistsNothing()
    {
        var (session, question) = await SeedSessionForAsync("owner");
        var ai = new FakeAiService();
        var sut = new SessionService(_db, ai);
        var request = new FinishSessionRequest(session.Id, [new AnswerDto(question.Id, "Attacker answer.")]);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => sut.FinishSession("attacker", request));

        Assert.False(ai.WasCalled);                             // no wasted/leaky AI call
        Assert.Empty(await _db.QuestionResults.ToListAsync());  // nothing graded or stored

        var untouched = await _db.Sessions.AsNoTracking().SingleAsync(s => s.Id == session.Id);
        Assert.Null(untouched.CompletedAt);                     // victim's session unchanged
    }

    // The grading model can return a questionId that was never submitted; that
    // must not crash the submission (BACKLOG P0 #3).
    [Fact]
    public async Task FinishSession_WhenAiReturnsUnknownQuestionId_PersistsOnlyAnsweredQuestions()
    {
        var (session, question) = await SeedSessionForAsync("owner");
        var ai = new FakeAiService(_ => new SessionSummaryDto(
        [
            new QuestionResultDto(question.Id, 7, "Graded."),
            new QuestionResultDto(9999, 5, "Phantom question never submitted."),
        ]));
        var sut = new SessionService(_db, ai);
        var request = new FinishSessionRequest(session.Id, [new AnswerDto(question.Id, "My answer.")]);

        await sut.FinishSession("owner", request);   // must not throw

        var saved = await _db.QuestionResults.ToListAsync();
        Assert.Single(saved);
        Assert.Equal(question.Id, saved[0].QuestionId);
    }
}
