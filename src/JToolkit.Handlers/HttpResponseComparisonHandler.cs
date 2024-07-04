using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Models;
using JToolkit.Http.Factory;
using JToolkit.Http.Mappers;
using JToolkit.Http.Models;

namespace JToolkit.Handlers;

public class HttpResponseComparisonHandler(IHttpResponseComparisonMapper mapper, IJsonComparer jsonComparer) : IHttpResponseComparisonHandler
{
    public ComparisonResult Handle(HttpResponseComparisonRequest request)
    {
        var actualResponse = new HttpRequestFactory(request.Actual).SendRequest();
        var expectedResponse = new HttpRequestFactory(request.Expected).SendRequest();
        var jsonComparison = mapper.Map(actualResponse, expectedResponse);
        return jsonComparer.Compare(jsonComparison);
    }
}