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
    [property: JsonPropertyName("token")] string        Token,
    [property: JsonPropertyName("fn")]    long          Fn,
    [property: JsonPropertyName("fd")]    long          Fd,
    [property: JsonPropertyName("fp")]    long          Fp,
    [property: JsonPropertyName("t")]     string        Timestamp,
    [property: JsonPropertyName("n")]     OperationType OperationType,
    [property: JsonPropertyName("s")]     double        Sum,
    [property: JsonPropertyName("qr")]
    [property: JsonConverter(typeof(IntToBoolConverter))]
    bool IsQr);
