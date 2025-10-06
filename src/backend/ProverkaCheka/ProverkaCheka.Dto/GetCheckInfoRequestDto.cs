using System.Text.Json.Serialization;
using Infrastructure.Utils.JsonConverters;

namespace ProverkaCheka.Dto;

public enum OperationType
{
    Prihod         = 1,
    VozvratPrihoda = 2,
    Rashod         = 3,
    VozvratRashoda = 4
}

public record GetCheckInfoRequestDto(
    [property: JsonPropertyName("token")]
    string Token,
    [property: JsonPropertyName("fn")]
    long FiscalStorageDeviceNumber,
    [property: JsonPropertyName("fd")]
    long FiscalDocumentNumber,
    [property: JsonPropertyName("fp")]
    long DocumentFiscalAttribute,
    [property: JsonPropertyName("t")]
    string Timestamp,
    [property: JsonPropertyName("n")]
    OperationType OperationType,
    [property: JsonPropertyName("s")]
    double Sum,
    [property: JsonPropertyName("qr"), JsonConverter(typeof(IntToBoolConverter))]
    bool IsQr);
