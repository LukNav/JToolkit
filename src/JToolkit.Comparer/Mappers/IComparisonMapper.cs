using JToolkit.Comparer.Models;

namespace JToolkit.Comparer.Mappers;

public interface IComparisonMapper
{
     ComparableObject Map(ComparisonRequest request);
}