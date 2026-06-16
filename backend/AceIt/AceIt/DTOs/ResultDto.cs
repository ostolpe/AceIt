namespace AceIt.DTOs;

public record ResultDto(List<QuestionResult> Results);
public record QuestionResult(int QuestionId, int Score, string Feedback);