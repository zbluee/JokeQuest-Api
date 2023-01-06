using System.ComponentModel.DataAnnotations;

namespace JobServices.Modles;

public class UserSignInDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}