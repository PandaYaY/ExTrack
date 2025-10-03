using Dapper;
using Infrastructure.DataTypes.Enums;
using Infrastructure.Utils.SqlTypeHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace ExTrack.Users.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddUsersService(this IServiceCollection services)
    {
        services.AddUsersRepository()
                .AddScoped<IUsersService, UsersService>();

        return services;
    }

    private static IServiceCollection AddUsersRepository(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        RegisterRepositoryTypes();
        return services;
    }

    private static void RegisterRepositoryTypes()
    {
        SqlMapper.AddTypeHandler(new EnumAsNumberTypeHandler<Role, short>());
    }
}
