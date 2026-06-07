namespace AceIt.DTOs;

public record SessionDto(int Id, IEnumerable<QuestionDto> Questions);
