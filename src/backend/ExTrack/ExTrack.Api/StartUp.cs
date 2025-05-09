using Dapper;
using ExTrack.Repositories;
using Serilog;
using Serilog.Exceptions;
using ExTrack.Service;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace ExTrack.Api;

public class StartUp
{
    private readonly bool _isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    private readonly WebApplicationBuilder _appBuilder;
    private readonly ConfigurationManager  _configuration;
    private readonly IServiceCollection    _services;

    public StartUp()
    {
        _appBuilder    = WebApplication.CreateBuilder();
        _configuration = _appBuilder.Configuration;
        _services      = _appBuilder.Services;
    }

    public WebApplication InitApplication()
    {
        ConfigureLogger();
        ConfigureDbs();
        ConfigureServices();

        return ConfigureWebApi();
    }

    private WebApplication ConfigureWebApi()
    {
        _services.AddControllers()
                 .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

        _services.AddEndpointsApiExplorer();
        _services.AddSwaggerGen();

        var app = _appBuilder.Build();

        // Add Swagger if is dev
        if (_isDev)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseRouting();

        return app;
    }

    private void ConfigureServices()
    {
        _services.AddCheckService(_configuration);

        _services.AddSingleton<IUsersRepository, UsersRepository>();
    }

    private void ConfigureLogger()
    {
        _appBuilder.Host.UseSerilog((context, services, configuration) =>
                                        configuration.ReadFrom.Configuration(context.Configuration)
                                                     .ReadFrom.Services(services)
                                                     .Enrich.WithExceptionDetails()
                                                     .Enrich.FromLogContext());
    }

    private void ConfigureDbs()
    {
        var connectionString = _configuration.GetConnectionString("ex_track");
        if (connectionString is null)
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string \"ex_track\" is null");
        }

        _services.AddTransient<QueryFactory>(sp =>
        {
            var connection = new NpgsqlConnection(connectionString);
            var compiler   = new PostgresCompiler();
            return new QueryFactory(connection, compiler);
        });

        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
