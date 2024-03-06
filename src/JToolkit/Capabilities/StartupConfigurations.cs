using JToolkit.Configuration.Models;

namespace JToolkit.Capabilities;

public static class StartupConfigurations
{
    public static void ConfigureConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDataSourceConfiguration>(GetDataSourceConfiguration(configuration));
        services.AddSingleton<IDomainConfiguration>(GetDomainContext(configuration));
    }
    
    private static DataSourceConfiguration GetDataSourceConfiguration(IConfiguration config)
    {
        string password = config["dataSource:password"] 
                        ?? throw new InvalidOperationException(ConfigurationNotFoundMessage("host", "dataSource"));
        string url = config["dataSource:url"] 
                     ?? throw new InvalidOperationException(ConfigurationNotFoundMessage("url", "dataSource"));

        return new DataSourceConfiguration
        {
            Url = url,
            Password = password
        };
    }
    
    private static DomainConfiguration GetDomainContext(IConfiguration config)
    {
        string[] requiredScopes = config.GetSection("domain:requiredScopes").Get<string[]>()
                                  ?? throw new InvalidOperationException(ConfigurationNotFoundMessage("requiredScopes", "domain"));

        return new DomainConfiguration
        {
            RequiredScopes = requiredScopes,
        };
    }
    
    private static string ConfigurationNotFoundMessage(string configName, string parentName) => $"Configuration '{configName}' in {parentName} was invalid or not found.";
}