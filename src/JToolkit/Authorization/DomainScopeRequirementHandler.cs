using JToolkit.Configuration.Models;
using Microsoft.AspNetCore.Authorization;

namespace JToolkit.Authorization;

public class DomainScopeRequirementHandler : AuthorizationHandler<ScopeRequirement>
{
    private readonly IDomainConfiguration _domainConfiguration;
    private readonly IHostEnvironment _environment;
    private const string ScopeClaimType = "scope";
        
    public DomainScopeRequirementHandler(IDomainConfiguration domainConfiguration, IHostEnvironment environment)
    {
        _domainConfiguration = domainConfiguration ?? throw new ArgumentNullException(nameof(domainConfiguration));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        // Auth not needed when running locally
        if (_environment.IsDevelopment())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        var requiredScopes = _domainConfiguration.RequiredScopes;

        if (!context.User.HasClaim(c => c.Type == ScopeClaimType))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var userScopes = context.User.FindAll(c => c.Type == ScopeClaimType)
            .Select(c => c.Value)
            .ToHashSet();

        // In case all scopes are allowed
        if (!requiredScopes.Any() || _environment.IsDevelopment())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (requiredScopes.Any(requiredScope => userScopes.Contains(requiredScope)))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
            
        context.Fail();
            
        return Task.CompletedTask;
    }
}