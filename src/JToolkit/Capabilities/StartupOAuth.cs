using JToolkit.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace JToolkit.Capabilities;

public static class StartupOAuth // TODO: Review if needed, after figuring out mapping
{
    public static IServiceCollection ConfigureOAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthCapability(configuration);
            
        return services;
    }

    private static void AddAuthCapability(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = configuration["OAuth:Audience"];
                options.Authority = configuration["OAuth:Authority"];
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromSeconds(300),
                    RequireSignedTokens = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["OAuth:Authority"]
                };
            });
    }

    public static IServiceCollection ConfigureDomainAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.ScopePolicy,
                policy => policy.Requirements.Add(new ScopeRequirement()));
        });
            
        services.AddSingleton<IAuthorizationHandler, DomainScopeRequirementHandler>();
            
        return services;
    }
}