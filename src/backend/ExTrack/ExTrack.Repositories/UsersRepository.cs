using Microsoft.Extensions.Configuration;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace ExTrack.Repositories;

public interface IUsersRepository
{
    Task<UserEntity?> GetUserById(int id);
}

public class UsersRepository(IConfiguration configuration) : IUsersRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("ex_track") ??
                                                throw new ArgumentNullException(
                                                    nameof(_connectionString),
                                                    "Connection string \"ex_track\" is null");

    public async Task<UserEntity?> GetUserById(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var db = new QueryFactory(connection, compiler);
        var query = await db.Query("users").Where("id", id).Select("id", "role_id", "login").FirstOrDefaultAsync<UserEntity>();
        return query;
    }
}

public record UserEntity(int Id, short RoleId, string Login);
