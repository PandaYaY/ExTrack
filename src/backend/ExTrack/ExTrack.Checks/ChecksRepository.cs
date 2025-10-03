using System.Text.Json;
using Dapper;
using ExTrack.Checks.Models;
using Npgsql;

namespace ExTrack.Checks;

public interface IChecksRepository
{
    Task<int> AddCheckInfo(CheckInfo checkInfo);
}

public class ChecksRepository(NpgsqlConnection connection) : IChecksRepository
{
    public Task<int> AddCheckInfo(CheckInfo checkInfo)
    {
        const string sql     = "select * from public.add_check_info(@json);";
        var          checkId = connection.QueryFirstAsync<int>(sql, new { json = JsonSerializer.Serialize(checkInfo) });
        return checkId;
    }
}
