using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JokeApi.Modles;

[BsonIgnoreExtraElements]
public class User {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    [BsonIgnoreIfNull]
    public string? Name { get; set; } 

    [BsonElement("email")]
    [Required]
    public string Email { get; set; } = null!;

    [BsonElement("password")]
    [Required]
    // [StringLength(maximumLength:30,MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [BsonElement("role")]
    public string Role {get; set; } = CustomRoles.Member;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}