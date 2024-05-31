namespace JToolkit.Comparer.Models;

public record ComparisonResult(bool AreEquivalent, List<Difference>? Differences); // TODO: change list to array