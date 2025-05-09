using System.Text.Json.Serialization;
using Infrastructure.Utils.JsonConverters;

namespace ProverkaCheka.Dto;

public record GetCheckInfoResponseDto(
    [property: JsonPropertyName("code")] ScanResultEnum StatusCode,
    [property: JsonPropertyName("first"), JsonConverter(typeof(IntToBoolConverter))]
    bool IsFirst,
    [property: JsonPropertyName("data")] CheckInfoDto Data,
    [property: JsonPropertyName("request")]
    RequestInfoDto RequestInfo);

public enum ScanResultEnum
{
    InCorrect       = 0,
    Success         = 1,
    NotSuccessYet   = 2,
    TooManyRequests = 3,
    PleaseWait      = 4,
    Another         = 5
}

public record CheckInfoDto(
    [property: JsonPropertyName("json")] CheckInfoJsonDto Json,
    [property: JsonPropertyName("html")] string           Html);

public record CheckInfoJsonDto(
    [property: JsonPropertyName("user")] string User,
    [property: JsonPropertyName("retailPlace")]
    string RetailPlace,
    [property: JsonPropertyName("retailPlaceAddress")]
    string RetailPlaceAddress,
    [property: JsonPropertyName("items")]  List<ItemDto> Items,
    [property: JsonPropertyName("nds10")]  long          Nds10,
    [property: JsonPropertyName("nds18")]  long          Nds18,
    [property: JsonPropertyName("fnsUrl")] string        FnsUrl,
    [property: JsonPropertyName("dateTime")]
    DateTime Timestamp,
    [property: JsonPropertyName("totalSum")]
    long Sum);

public record ItemDto(
    [property: JsonPropertyName("quantity")]
    double Quantity,
    [property: JsonPropertyName("price")]  long   Price,
    [property: JsonPropertyName("sum")]    long   Sum,
    [property: JsonPropertyName("nds")]    long   Nds,
    [property: JsonPropertyName("ndsSum")] long   NdsSum,
    [property: JsonPropertyName("name")]   string Name);

public record RequestDataDto(
    [property: JsonPropertyName("fn")] string Fn,
    [property: JsonPropertyName("fd")] string Fd,
    [property: JsonPropertyName("fp")] string Fp,
    [property: JsonPropertyName("check_time")]
    string Timestamp,
    [property: JsonPropertyName("type")] string OperationType,
    [property: JsonPropertyName("sum")]  string Sum);

public record RequestInfoDto(
    [property: JsonPropertyName("qrurl")]  string         QrUrl,
    [property: JsonPropertyName("qrfile")] string         QrFileInfo,
    [property: JsonPropertyName("qrraw")]  string         QrRaw,
    [property: JsonPropertyName("manual")] RequestDataDto Input);
