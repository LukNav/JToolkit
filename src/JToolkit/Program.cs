using Microsoft.AspNetCore;

namespace JToolkit;

public static class Program
{
    public static void Main()
    {
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        // const int minThreads = 100;
        // ThreadPool.SetMinThreads(minThreads, minThreads);
        
        BuildWebHost()
            .Run();
    }

    private static IWebHost BuildWebHost()
    {
        return WebHost.CreateDefaultBuilder()
            .SuppressStatusMessages(true)
            .ConfigureServices(services =>
            {
                services.AddSingleton(s => s.GetRequiredService<IHostEnvironment>().ConfigureConfig());
            })
            .UseStartup<Startup>()
            .UseShutdownTimeout(TimeSpan.FromSeconds(800))
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                // builder.AddProvider(new TitanLoggerProvider(new TitanLogger("AppLogger")));
                builder.SetMinimumLevel(LogLevel.Warning);
            })
            .Build();
    }

    private static IConfiguration ConfigureConfig(this IHostEnvironment environment)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder();

        return builder
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("Configuration/appsettings.json")
            .AddJsonFile($"Configuration/appsettings.{environment.EnvironmentName}.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }
}