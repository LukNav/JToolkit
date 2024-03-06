using JToolkit.Comparer.Exceptions;
using JToolkit.Comparer.Models;
using Newtonsoft.Json.Linq;

namespace JToolkit.Comparer.Mappers;

public class ComparisonMapper : IComparisonMapper
{
    public ComparableObject Map(ComparisonRequest request)
    {
        if(request.Actual is null || request.Expected is null)
            throw new InvalidJsonComparisonRequestException("Invalid Request", "ActulJson and ExpectedJson fields are required."); // TODO: Exception middleware

        var actual = JObject.Parse(request.Actual.ToString()!); // TODO: Fix suppresion ('!') operator. Decide if the mapper could map to custom object, so we could compare ANYTHING, not only json
        var expected = JObject.Parse(request.Expected.ToString()!);
        return new ComparableObject(actual, expected);
    }
}