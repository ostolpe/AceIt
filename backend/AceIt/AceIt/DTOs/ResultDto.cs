namespace AceIt.DTOs;

public record SessionSummaryDto(List<QuestionResultDto> Results)
{
    public double AverageScore => Results.Count == 0 ? 0 : (double)Results.Sum(x => x.Score) / Results.Count;
};

public record QuestionResultDto(int QuestionId, int Score, string Feedback);
