using Dapper;
using ExTrack.Checks.Models;
using Infrastructure.Utils.SqlTypeHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProverkaCheka.Client;

namespace ExTrack.Checks.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        // Регистрация AccessToken через IOptions<T>
        services.Configure<ProverkaChekaClientConfiguration>(configuration.GetSection("ProverkaCheka"));

        services.AddScoped<IChecksService, ChecksService>()
                .AddChecksRepository()
                .AddProverkaChekaClient(configuration);

        return services;
    }

    private static IServiceCollection AddChecksRepository(this IServiceCollection services)
    {
        services.AddScoped<IChecksRepository, ChecksRepository>();
        RegisterChecksRepositoryTypes();
        return services;
    }

    private static void RegisterChecksRepositoryTypes()
    {
        SqlMapper.AddTypeHandler(typeof(List<ProductEntity>), new JsonTypeHandler<List<ProductEntity>>());
    }
}
