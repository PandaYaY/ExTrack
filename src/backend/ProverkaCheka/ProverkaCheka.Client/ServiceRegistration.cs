using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ProverkaCheka.Client;

public static class ServiceRegistration
{
    public static IServiceCollection AddProverkaChekaClient(this IServiceCollection services,
                                                                 IConfiguration          configuration)
    {
        var config = configuration.GetSection("ProverkaCheka").Get<ProverkaCheckaClientConfiguration>() ??
                     throw new ArgumentNullException(nameof(configuration), "ProverkaCheka configuration is missing");

        services.AddRefitClient<IProverkaChekaClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = config.BaseUrl;
                    c.Timeout     = config.Timeout;
                });

        return services;
    }
}
