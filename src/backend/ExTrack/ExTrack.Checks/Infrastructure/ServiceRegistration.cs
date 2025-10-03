using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProverkaCheka.Client;

namespace ExTrack.Checks.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddChecksService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChecksService, ChecksService>()
                .AddScoped<IChecksRepository, ChecksRepository>()
                .AddProverkaChekaClient(configuration);
        return services;
    }
}
