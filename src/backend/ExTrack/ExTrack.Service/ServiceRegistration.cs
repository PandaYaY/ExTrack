using ExTrack.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProverkaCheka.Client;

namespace ExTrack.Service;

public static class ServiceRegistration
{
    public static IServiceCollection AddCheckService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICheckRepository, CheckRepository>();
        services.AddProverkaChekaClient(configuration);
        return services;
    }
}
