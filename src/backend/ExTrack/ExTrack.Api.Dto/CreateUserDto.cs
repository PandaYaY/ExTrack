using System.Text.Json.Serialization;
using Infrastructure.DataTypes.Enums;

namespace ExTrack.Api.Dto;

public record CreateUserDto(
        [property: JsonPropertyName("role_id")]
        Role RoleId,
        [property: JsonPropertyName("login")]
        string Login,
        [property: JsonPropertyName("password_hash")]
        string PasswordHash);
