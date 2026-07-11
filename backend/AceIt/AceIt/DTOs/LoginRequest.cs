using System.ComponentModel.DataAnnotations;

namespace AceIt.DTOs;

public record LoginRequest(
    [Required]
    [EmailAddress]
    string Email,
    [Required]
    string Password
    );

