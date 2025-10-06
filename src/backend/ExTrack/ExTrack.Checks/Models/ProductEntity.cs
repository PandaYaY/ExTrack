using System.Text.Json.Serialization;

namespace ExTrack.Checks.Models;

public record ProductEntity(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("product_id")]
    int ProductId,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("true_name")]
    string TrueName,
    [property: JsonPropertyName("price")]
    double Price,
    [property: JsonPropertyName("quantity")]
    double Quantity,
    [property: JsonPropertyName("total_price")]
    double TotalPrice);
