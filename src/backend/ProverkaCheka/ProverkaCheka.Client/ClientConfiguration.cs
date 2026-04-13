namespace ProverkaCheka.Client;

public record ProverkaChekaClientConfiguration
{
    public required Uri    BaseUrl        { get; init; }
    public required short  TimeoutSeconds { get; init; }
    public required string AccessToken    { get; init; }
}
