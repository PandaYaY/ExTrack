using System.Text.Json.Serialization;

namespace ExTrack.Checks.Models;

public record CheckInfo(
    [property: JsonPropertyName("shop_name")]
    string ShopName,
    [property: JsonPropertyName("shop_address")]
    string ShopAddress,
    [property: JsonPropertyName("check_date")]
    DateTime CheckDate,
    [property: JsonPropertyName("total_sum")]
    double TotalSum,
    [property: JsonPropertyName("products")]
    List<CheckProduct> Products);

public record CheckProduct(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("price")]
    double Price,
    [property: JsonPropertyName("total_price")]
    double TotalPrice,
    [property: JsonPropertyName("quantity")]
    double Quantity);
