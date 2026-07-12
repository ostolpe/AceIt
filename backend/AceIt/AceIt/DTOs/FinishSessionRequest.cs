using System.ComponentModel.DataAnnotations;

namespace AceIt.DTOs;

public record FinishSessionRequest(
    [Range(1, int.MaxValue)]
    int SessionId,
    [Required]
    [MinLength(1)]
    IEnumerable<AnswerDto> Answers);
