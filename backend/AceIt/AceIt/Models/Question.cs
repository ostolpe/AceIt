namespace AceIt.Models;

public class Session
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Question> Questions { get; set; } = [];
    public List<QuestionResult> Results { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

public class Question
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public Topic Topic { get; set; }

    public List<Session> Sessions { get; set; } = [];
}

public class QuestionResult
{
    public int Id { get; set; }
    public required string UserAnswer { get; set; }
    public required string Feedback { get; set; }
    public int Score { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}

public enum Topic
{
    CSharp,
    DotNet,
    OOP,
    Collections,
    Linq,
    ErrorHandling,
    Testing
}