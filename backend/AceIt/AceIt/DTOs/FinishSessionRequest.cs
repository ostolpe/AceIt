namespace AceIt.DTOs;

public record FinishSessionRequest(int SessionId, IEnumerable<AnswerDto> Answers);