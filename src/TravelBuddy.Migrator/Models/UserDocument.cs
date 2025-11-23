using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class UserDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int LegacyUserId { get; set;}

    public string Name { get; set;} = default!;
    public string Email { get; set; } = default!;
    public string? PasswordHash { get; set; }
    public DateOnly? Birthdate { get; set; }

    public bool IsDeleted { get; set; }
    public string Role { get; set; } = "user";
}