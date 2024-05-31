using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Comparer.Models;

namespace JToolkit.Handlers;

public class ComparisonHandler(IComparisonMapper comparisonMapper, IJsonComparer jsonComparer) : IComparisonHandler
{
    public ComparisonResult Handle(ComparisonRequest request)
    {
        // TODO: validate in base or here ?? Validation should be done before the request comes to handler? Injected middleware
        var jsonComparison = comparisonMapper.Map(request);
        return jsonComparer.Compare(jsonComparison);
    }
}