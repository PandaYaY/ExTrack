using Dapper;
using ExTrack.Users.Models;
using Infrastructure.DataTypes.Enums;
using Npgsql;

namespace ExTrack.Users;

public interface IUsersRepository
{
    Task<UserEntity?> GetUserById(int id);
    Task<UserEntity>  CreateUser(Role roleId, string login, string passwordHash);
}

public class UsersRepository(NpgsqlConnection connection) : IUsersRepository
{
    public async Task<UserEntity?> GetUserById(int id)
    {
        const string sql  = "select id, role_id, login, hash_password from users where id = @id";
        var          user = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { id });
        return user;
    }

    public async Task<UserEntity> CreateUser(Role roleId, string login, string passwordHash)
    {
        const string sql = "select * from public.create_user(@login, @passwordHash, @roleId)";
        var userId = await connection.QuerySingleAsync<int>(sql, new { login, passwordHash, roleId = (short)roleId });
        var user = new UserEntity(userId, roleId, login, passwordHash);
        return user;
    }
}
