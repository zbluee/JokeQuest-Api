using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

public class UserPointDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
     public int Point { get; set; }
}