using System.Text.Json.Serialization;
using Infrastructure.DataTypes.Enums;

namespace ExTrack.Users.Models;

public record UserEntity(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("role_id")]
    Role RoleId,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("password_hash")]
    string HashPassword);