using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JobServices.Modles;

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
    public string Password { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}