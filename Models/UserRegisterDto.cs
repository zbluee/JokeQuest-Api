using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

public class UserRegisterDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}