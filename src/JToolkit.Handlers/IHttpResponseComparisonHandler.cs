using JToolkit.Comparer.Models;
using JToolkit.Http;
using JToolkit.Http.Models;

namespace JToolkit.Handlers;

public interface IHttpResponseComparisonHandler
{
    ComparisonResult Handle(HttpResponseComparisonRequest request);
}