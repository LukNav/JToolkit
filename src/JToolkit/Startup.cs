using CorrelationId;
using CorrelationId.DependencyInjection;
using JToolkit.Capabilities;
using JToolkit.Middleware;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace JToolkit;

public class Startup
{
    private IConfiguration Configuration { get; }


    public Startup(IConfiguration configuration)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); // TODO: Primary constructor? (Use resharper suggestion)
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultCorrelationId(options =>
            {
                options.UpdateTraceIdentifier = false;
                options.IncludeInResponse = true;
                options.RequestHeader = "CorrelationId";
                options.ResponseHeader = "CorrelationId";
            }
        );
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
        });

        services.ConfigureValidations();

        services.ConfigureConfigurations(Configuration);
        services.ConfigureServices();
        // services.AddRepositories();
        // services.AddDataProviders();

        services.ConfigureOAuth(Configuration);
        services.ConfigureDomainAuthorization();
        
        services.AddEndpointsApiExplorer();
        
        services.ConfigureMvc();
        services.AddResponseCompression();
        
        services.AddSwaggerGen();
        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseCorrelationId();

        // Routing must be used before UseExtendedHttpMetrics (for labels as controller, action)
        app.UseRouting();

        // Use of error handler order is important, but it must be after UseExtendedHttpMetrics
        // in order to ensure that prometheus-net reports the correct HTTP response status code (or records some errors at all).
        app.UseErrorHandler();
        app.UseMiddleware<ExceptionWrapperMiddleware>();

        // app.UseMiddleware<QueryLogMiddleware>();
        app.UseSwagger(env);
        // Must be before UseAuthorization()
        app.UseMiddleware<AuthorizationHeaderFilterMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseResponseCompression();

        app.UseHealthChecks("/health");
    }
}