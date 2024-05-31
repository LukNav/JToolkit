namespace JToolkit.Comparer.Models;

public record ComparisonResult(bool AreEquivalent, List<Difference>? Differences); // TOOD: change list to array