using System.Text.Json;
using Dapper;
using ExTrack.Checks.Models;
using Npgsql;

namespace ExTrack.Checks;

public interface IChecksRepository
{
    Task<CheckEntity?>      GetCheckById(int  checkId);
    Task<List<CheckEntity>> GetUserChecks(int userId, int       page, int perPage);
    Task<int>               AddCheckInfo(int  userId, CheckInfo checkInfo);
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
                                    from ex_track.public.checks c
                                             join ex_track.public.shops s on c.shop_id = s.id
                                             left join ex_track.public.true_shops ts on s.true_shop_id = ts.id
                                             join ex_track.public.check_products cp on c.id = cp.check_id
                                             join public.products p on cp.product_id = p.id
                                             left join public.true_products tp on p.true_product_id = tp.id
                                    where ({0})
                                    group by c.id, c.date, s.id, ts.id
                                    order by c.date desc
                                    {1}
                                    """;


    private static readonly string OneCheckSql = string.Format(CheckSql, "id = @checkId", string.Empty);

    private static readonly string UserChecksSql =
        string.Format(CheckSql, "user_id = @userId", "limit @limit offset @offset");

    public async Task<CheckEntity?> GetCheckById(int checkId)
    {
        var check = await connection.QueryFirstOrDefaultAsync<CheckEntity>(OneCheckSql, new { checkId });
        return check;
    }

    public async Task<List<CheckEntity>> GetUserChecks(int userId, int page, int perPage)
    {
        var offset = perPage * (page - 1);
        var checks = await connection.QueryAsync<CheckEntity>(UserChecksSql, new { userId, limit = perPage, offset });
        return checks.ToList();
    }

    public async Task<int> AddCheckInfo(int userId, CheckInfo checkInfo)
    {
        const string sql = "select * from public.add_check_info(@userId::integer, @json::jsonb);";
        var checkId =
            await connection.QueryFirstAsync<int>(sql, new { userId, json = JsonSerializer.Serialize(checkInfo) });
        return checkId;
    }
}
