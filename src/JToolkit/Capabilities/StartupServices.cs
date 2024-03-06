namespace JToolkit.Capabilities;

public static class StartupConfiguration
{
    // public static IServiceCollection ConfigureServices(this IServiceCollection services)
    // {
    //     // services.AddSingleton<IStatsRequestHandler, StatsRequestHandler>();
    //    
    //     return services;
    // }

    public static void ConfigureValidations(this IServiceCollection services)
    {
        services.ConfigureValidationRules();
    }
}