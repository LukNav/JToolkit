using JToolkit.Comparer.Models;

namespace JToolkit.Http.Mappers;

public interface IHttpResponseComparisonMapper
{
     ComparableObject Map(Task<HttpResponseMessage> actual, Task<HttpResponseMessage> expected);
}