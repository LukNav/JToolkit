using JToolkit.Comparer.Models;

namespace JToolkit.Handlers;

public interface IComparisonHandler
{
    ComparisonResult Handle(ComparisonRequest request);
}