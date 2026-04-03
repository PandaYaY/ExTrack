using Dapper;
using ExTrack.Checks.Infrastructure;
using Npgsql;
using Serilog;
using Serilog.Exceptions;

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
        // Настройка CORS
        _services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        _services.AddControllers()
                 .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

        _services.AddApiVersioning(options =>
                  {
                      // reporting api versions will return the headers
                      // "api-supported-versions" and "api-deprecated-versions"
                      options.ReportApiVersions = true;
                  })
                 .AddMvc();

        _services.AddEndpointsApiExplorer();
        _services.AddSwaggerGen();

        var app = _appBuilder.Build();

        // Использование CORS
        app.UseCors("AllowFrontend");

        // Add Swagger if is dev
        if (_isDev)
        {
            app.UseSwagger(); // TODO scalar
        }

        app.UseRouting();
        app.MapControllers();

        return app;
    }

    private void ConfigureServices()
    {
        _services.AddChecksService(_configuration);
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

        _services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
