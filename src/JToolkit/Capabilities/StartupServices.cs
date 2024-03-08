using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Handlers;

namespace JToolkit.Capabilities;

public static class StartupConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IComparisonHandler, ComparisonHandler>();
        services.AddSingleton<IJsonComparer, JsonComparerV2>();
        services.AddSingleton<IComparisonMapper, ComparisonMapper>();
        return services;
    }

    public static void ConfigureValidations(this IServiceCollection services)
    {
        services.ConfigureValidationRules();
    }
}