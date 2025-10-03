using System.Data;
using Dapper;

namespace Infrastructure.Utils.SqlTypeHandlers;

public class EnumAsNumberTypeHandler<TEnum, TNumber> : SqlMapper.TypeHandler<TEnum> where TEnum : struct, Enum
{
    public override void SetValue(IDbDataParameter parameter, TEnum value)
    {
        parameter.Value = value;
        switch (typeof(TNumber).Name)
        {
            case "Int16":
                parameter.DbType = DbType.Int16;
                break;
            case "Int32":
                parameter.DbType = DbType.Int32;
                break;
            case "Int64":
                parameter.DbType = DbType.Int64;
                break;
            default:
                throw new ArgumentException($"Unsupported number type: {typeof(TNumber).Name}");
        }
    }

    public override TEnum Parse(object value)
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), Convert.ChangeType(value, typeof(TNumber)));
    }
}
