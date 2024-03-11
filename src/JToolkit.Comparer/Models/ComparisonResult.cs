namespace JToolkit.Comparer.Models;

public record ComparisonResult(bool AreEquivalent, List<string>? Differences); // TOOD: change list to array