using System.Text.Json.Serialization;

namespace ExTrack.Checks.Models;

public record CheckEntity(
    [property: JsonPropertyName("check_id")]
    int CheckId,
    [property: JsonPropertyName("check_date")]
    DateTime CheckDate,
    [property: JsonPropertyName("shop_id")]
    int ShopId,
    [property: JsonPropertyName("shop_name")]
    string ShopName,
    [property: JsonPropertyName("true_shop_name")]
    string TrueShopName,
    [property: JsonPropertyName("shop_address")]
    string ShopAddress,
    [property: JsonPropertyName("products")]
    List<ProductEntity> Products);
