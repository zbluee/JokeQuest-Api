using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

public class Joke {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("jokeQuestion")]
    [Required]
    public string JokeQuestion { get; set; } = null!;
    [BsonElement("jokeAnswer")]
    [Required]
    public string JokeAnswer { get; set; } = null!;
    [BsonElement("createdBy")]
    public string? CreatedBy { get; set; } 

    [BsonElement("jokePoints")]
    public int JokePoints { get; set; } 

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}