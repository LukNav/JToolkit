namespace JToolkit.Configuration.Models;

public interface IDataSourceConfiguration
{
    string Url { get; }
    string Password { get; }

}

public record DataSourceConfiguration : IDataSourceConfiguration
{
    public string Url { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}