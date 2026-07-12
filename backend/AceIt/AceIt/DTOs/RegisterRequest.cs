using System.ComponentModel.DataAnnotations;

namespace AceIt.DTOs;

public record RegisterRequest(
        [Required] [EmailAddress] string Email, 
        [Required] string Password);
