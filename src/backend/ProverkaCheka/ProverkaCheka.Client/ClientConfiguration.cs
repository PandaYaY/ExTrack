namespace ProverkaCheka.Client;

public record ProverkaCheckaClientConfiguration
{
    public required Uri      BaseUrl     { get; init; }
    public required TimeSpan Timeout     { get; init; }
    public required string   AccessToken { get; init; }
}
