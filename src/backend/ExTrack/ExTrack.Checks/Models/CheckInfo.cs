using System.Text.Json.Serialization;
using ProverkaCheka.Dto;

namespace ExTrack.Checks.Models;

public record CheckInfo (
    [property: JsonPropertyName("fiscal_storage_device_number")]
    long FiscalStorageDeviceNumber,
    [property: JsonPropertyName("fiscal_document_number")]
    long FiscalDocumentNumber,
    [property: JsonPropertyName("document_fiscal_attribute")]
    long DocumentFiscalAttribute,
    [property: JsonPropertyName("operation_type")]
    OperationType OperationType,
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
