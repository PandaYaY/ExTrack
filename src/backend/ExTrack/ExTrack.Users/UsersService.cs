using System.Text.RegularExpressions;
using ExTrack.Users.Models;
using Infrastructure.DataTypes.Enums;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ExTrack.Users;

public interface IUsersService
{
    Task<UserEntity?> GetUserById(int userId);
    Task<UserEntity>  CreateUser(Role role, string login, string passwordHash);
}

public partial class UsersService(ILogger<UsersService> logger, IUsersRepository repository) : IUsersService
{
    public async Task<UserEntity?> GetUserById(int userId)
    {
        logger.LogInformation("Getting user with ID {userId}", userId);
        return await repository.GetUserById(userId);
    }

    public async Task<UserEntity> CreateUser(Role role, string login, string passwordHash)
    {
        try
        {
            logger.LogInformation("Create user ({role}, {login})", role, login);
            return await repository.CreateUser(role, login, passwordHash);
        }
        catch (PostgresException exception)
        {
            logger.LogWarning(exception, "Create user PostgresException");
            if (ExistingUserRegexp().IsMatch(exception.MessageText))
            {
                throw new ArgumentException(exception.MessageText, nameof(login));
            }

            if (NonExistingRole().IsMatch(exception.MessageText))
            {
                throw new ArgumentException(exception.MessageText, nameof(role));
            }

            logger.LogError(exception, "Unknown PostgresException while create user");
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Create user error");
            throw;
        }
    }

    [GeneratedRegex("""Пользователь с логином ".*" уже существует""")]
    private static partial Regex ExistingUserRegexp();

    [GeneratedRegex("""Пользователь с логином ".*" уже существует""")]
    private static partial Regex NonExistingRole();
}
