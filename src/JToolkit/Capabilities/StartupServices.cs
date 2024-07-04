using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Handlers;
using JToolkit.Http.Mappers;

namespace JToolkit.Capabilities;

public static class StartupConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IComparisonHandler, ComparisonHandler>();
        services.AddSingleton<IComparisonMapper, ComparisonMapper>();
        services.AddSingleton<IJsonComparer, JsonComparer>();
        services.AddSingleton<IHttpResponseComparisonMapper, HttpResponseComparisonMapper>();
        services.AddSingleton<IHttpResponseComparisonHandler, HttpResponseComparisonHandler>();
        return services;
    }

    public static void ConfigureValidations(this IServiceCollection services)
    {
        services.ConfigureValidationRules();
    }
}