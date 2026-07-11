using System.ComponentModel.DataAnnotations;

namespace AceIt.DTOs;

public record FinishSessionRequest(
    [Required]
    int SessionId,
    [Required]
    IEnumerable<AnswerDto> Answers
    );