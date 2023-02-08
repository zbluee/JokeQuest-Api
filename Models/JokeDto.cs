using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

public class JokeDto {

    [Required]
    public string JokeQuestion { get; set; } = null!;
    [Required]
    public string JokeAnswer { get; set; } = null!;
}