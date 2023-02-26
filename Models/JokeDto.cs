using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

public class JokeDto {

    [Required]
    public string JokeQuestion { get; set; } = null!;
    [Required]
    public string JokeAnswer { get; set; } = null!;
    
    [Range(1, 10, ErrorMessage = "Joke point must be between 1 and 10")]
    public int JokePoints { get; set; } = 5;
}