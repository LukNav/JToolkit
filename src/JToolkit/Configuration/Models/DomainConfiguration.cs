using System.Collections.Immutable;

namespace JToolkit.Configuration.Models;

public interface IDomainConfiguration
{
    public IReadOnlyCollection<string> RequiredScopes { get; }
}

public record DomainConfiguration : IDomainConfiguration
{
    public IReadOnlyCollection<string> RequiredScopes { get; init; } = ImmutableList<string>.Empty;
}