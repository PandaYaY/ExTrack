using System.Data;
using System.Text.Json;
using Dapper;
using ExTrack.Api.Dto.Checks;
using ExTrack.Checks.Models;
using Npgsql;

namespace ExTrack.Checks;

public interface IChecksRepository
{
    Task<CheckEntity?>      GetCheckById(int                 checkId);
    Task<CheckEntity?>      GetCheckByParams(GetCheckInfoDto checkParams);
    Task<List<CheckEntity>> GetUserChecks(int                userId, int       page, int perPage);
    Task<CheckEntity>       AddCheckInfo(int                 userId, CheckInfo checkInfo);
}

public class ChecksRepository(NpgsqlConnection connection) : IChecksRepository
{
    private const string CheckSql = """
                                    select c.id as check_id,
                                           c.date as check_date,
                                           c.shop_id,
                                           s.name as shop_name,
                                           ts.name as true_shop_name,
                                           c.shop_address,
                                           to_jsonb(
                                                   array_agg(
                                                           jsonb_build_object(
                                                                   'id', cp.id,
                                                                   'product_id', p.id,
                                                                   'name', p.name,
                                                                   'true_name', tp.name,
                                                                   'price', cp.price,
                                                                   'quantity', cp.quantity,
                                                                   'total_price', cp.total_price
                                                           )
                                                   )
                                           )       as products
                                    from public.checks c
                                             join public.shops s on c.shop_id = s.id
                                             left join public.true_shops ts on s.true_shop_id = ts.id
                                             join public.check_products cp on c.id = cp.check_id
                                             join public.products p on cp.product_id = p.id
                                             left join public.true_products tp on p.true_product_id = tp.id
                                    where ({0})
                                    group by c.id, c.date, s.id, ts.id
                                    {1}
                                    """;

    private static readonly string OneCheckSql = string.Format(CheckSql, "c.id = @checkId", string.Empty);

    private static readonly string UserChecksSql =
        string.Format(CheckSql, "user_id = @userId", "order by c.date desc limit @limit offset @offset");

    public async Task<CheckEntity?> GetCheckById(int checkId)
    {
        var check = await GetCheckById(checkId, null);
        return check;
    }

    private async Task<CheckEntity?> GetCheckById(int checkId, IDbTransaction? transaction)
    {
        var check = await connection.QueryFirstOrDefaultAsync<CheckEntity>(OneCheckSql, new { checkId }, transaction);
        return check;
    }

    private static readonly string CheckByParamsSql =
        string.Format(CheckSql,
                      "c.fiscal_storage_device_number = @fn and c.fiscal_document_number = @fd and c.document_fiscal_attribute = @fp and c.operation_type = @ot",
                      string.Empty);

    public async Task<CheckEntity?> GetCheckByParams(GetCheckInfoDto checkParams)
    {
        var check = await connection.QueryFirstOrDefaultAsync<CheckEntity>(CheckByParamsSql,
                                                                           new
                                                                           {
                                                                               fn = checkParams
                                                                                   .FiscalStorageDeviceNumber,
                                                                               fd = checkParams.FiscalDocumentNumber,
                                                                               fp = checkParams.DocumentFiscalAttribute,
                                                                               ot = checkParams.OperationType
                                                                           });
        return check;
    }

    public async Task<List<CheckEntity>> GetUserChecks(int userId, int page, int perPage)
    {
        var offset = perPage * (page - 1);
        var checks = await connection.QueryAsync<CheckEntity>(UserChecksSql, new { userId, limit = perPage, offset });
        return checks.ToList();
    }

    public async Task<CheckEntity> AddCheckInfo(int userId, CheckInfo checkInfo)
    {
        const string sql = "select * from public.add_check_info(@userId::integer, @json::jsonb);";

        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();
        var checkId = await connection.QuerySingleAsync<int>(sql, new
                                                                  {
                                                                      userId, json = JsonSerializer.Serialize(checkInfo)
                                                                  }, transaction);
        var newCheckData = await GetCheckById(checkId, transaction);
        if (newCheckData is not null)
            await transaction.CommitAsync();
        else
            throw new ArgumentException("check is not added", nameof(checkInfo));
        return newCheckData;
    }
}
