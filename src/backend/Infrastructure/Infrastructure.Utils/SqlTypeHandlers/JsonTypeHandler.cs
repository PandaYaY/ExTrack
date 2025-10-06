using System.Data;
using System.Text.Json;
using Dapper;

namespace Infrastructure.Utils.SqlTypeHandlers;

public class JsonTypeHandler<T> : SqlMapper.ITypeHandler
{
    public void SetValue(IDbDataParameter parameter, object value)
    {
        parameter.Value  = JsonSerializer.Serialize(value);
        parameter.DbType = DbType.String; // Ensure the database type is string/text
    }

    public object? Parse(Type destinationType, object? value)
    {
        return value is null or DBNull ? default : JsonSerializer.Deserialize<T>((string)value);
    }
}
