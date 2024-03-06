using JToolkit.Comparer.Models;

namespace JToolkit.Comparer.Comparer;

public interface IJsonComparer
{
    ComparisonResult Compare(ComparableObject request);
}