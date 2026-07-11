using AceIt.Data;
using AceIt.DTOs;
using AceIt.Exceptions;
using AceIt.Models;
using AceIt.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Tests;

public class SessionServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;

    public SessionServiceTests()
    {
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

    private sealed class FakeAiService(SessionSummaryDto response) : IAiService
    {
        public bool WasCalled { get; private set; }

        public Task<SessionSummaryDto> GradeSession(FinishSessionRequest request)
        {
            WasCalled = true;
            return Task.FromResult(response);
        }
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
        var aiResponse = new SessionSummaryDto([new QuestionResultDto(question.Id, 8, "Solid answer.")]);
        var ai = new FakeAiService(aiResponse);
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

    [Fact]
    public async Task FinishSession_WhenCallerDoesNotOwnSession_ThrowsAndPersistsNothing()
    {
        var (session, question) = await SeedSessionForAsync("owner");
        var ai = new FakeAiService(new SessionSummaryDto([]));
        var sut = new SessionService(_db, ai);
        var request = new FinishSessionRequest(session.Id, [new AnswerDto(question.Id, "Attacker answer.")]);

        await Assert.ThrowsAsync<NotFoundException>(
            () => sut.FinishSession("attacker", request));

        Assert.False(ai.WasCalled);
        Assert.Empty(await _db.QuestionResults.ToListAsync());

        var untouched = await _db.Sessions.AsNoTracking().SingleAsync(s => s.Id == session.Id);
        Assert.Null(untouched.CompletedAt);
    }

    [Fact]
    public async Task FinishSession_WhenAiReturnsUnknownQuestionId_PersistsOnlyAnsweredQuestions()
    {
        var (session, question) = await SeedSessionForAsync("owner");
        var aiResponse = new SessionSummaryDto(
        [
            new QuestionResultDto(question.Id, 7, "Graded."),
            new QuestionResultDto(9999, 5, "Phantom question never submitted."),
        ]);
        var ai = new FakeAiService(aiResponse);
        var sut = new SessionService(_db, ai);
        var request = new FinishSessionRequest(session.Id, [new AnswerDto(question.Id, "My answer.")]);

        await sut.FinishSession("owner", request);

        var saved = await _db.QuestionResults.ToListAsync();
        Assert.Single(saved);
        Assert.Equal(question.Id, saved[0].QuestionId);
    }
}
