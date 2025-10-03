using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Utils.JsonConverters;

public class IntToBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Number:
                // Если значение - число, проверяем, равно ли оно 1
                var number = reader.GetInt32();
                return number == 1;
            case JsonTokenType.String:
                // Если значение - строка, проверяем, равна ли она "1"
                var stringValue = reader.GetString();
                return stringValue == "1";
            case JsonTokenType.None:
            case JsonTokenType.StartObject:
            case JsonTokenType.EndObject:
            case JsonTokenType.StartArray:
            case JsonTokenType.EndArray:
            case JsonTokenType.PropertyName:
            case JsonTokenType.Comment:
            case JsonTokenType.Null:
            default:
                throw new JsonException($"Неподдерживаемый тип для преобразования в bool: {reader.TokenType}");
        }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}
