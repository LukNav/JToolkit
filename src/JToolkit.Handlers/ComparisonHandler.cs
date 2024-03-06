using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Comparer.Models;

namespace JToolkit.Handlers;

public class ComparisonHandler : IComparisonHandler
{
    private IComparisonMapper _comparisonMapper;
    private IJsonComparer _jsonComparer;

    public ComparisonHandler(IComparisonMapper comparisonMapper, IJsonComparer jsonComparer)
    {
        _comparisonMapper = comparisonMapper;
        _jsonComparer = jsonComparer;
    }

    public ComparisonResult Handle(ComparisonRequest request)
    {
        // TODO: validate in base or here
        var jsonComparison = _comparisonMapper.Map(request);
        return _jsonComparer.Compare(jsonComparison);
    }
}