using System.Text.Json.Serialization;
using ProverkaCheka.Dto;

namespace ExTrack.Api.Dto.Checks;

public record GetCheckInfoDto(
    [property: JsonPropertyName("user_id")]
    int UserId,
    [property: JsonPropertyName("fiscal_storage_device_number")]
    long FiscalStorageDeviceNumber,
    [property: JsonPropertyName("fiscal_document_number")]
    long FiscalDocumentNumber,
    [property: JsonPropertyName("document_fiscal_attribute")]
    long DocumentFiscalAttribute,
    [property: JsonPropertyName("timestamp")]
    string Timestamp,
    [property: JsonPropertyName("operation_type")]
    OperationType OperationType,
    [property: JsonPropertyName("sum")]
    double Sum);
