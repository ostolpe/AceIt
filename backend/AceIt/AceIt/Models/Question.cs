namespace AceIt.Models;

public class Question
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public Topic Topic { get; set; }

    public List<Session> Sessions { get; set; } = [];
    // difficulty later
}

public class Session
{
    public int Id { get; set; }
    
    public List<Question> Questions { get; set; } = [];
    //userId later
}
public class AiResponse
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public required string Text { get; set; }
    public float Score { get; set; }
}

public enum Topic
{
   CSharp, DotNet 
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}
