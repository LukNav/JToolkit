using JToolkit.Comparer.Models;
using Newtonsoft.Json.Linq;

namespace JToolkit.Http.Mappers;

public class HttpResponseComparisonMapper : IHttpResponseComparisonMapper
{
    public ComparableObject Map(Task<HttpResponseMessage> actual, Task<HttpResponseMessage> expected)
    {
        var actualResponse = ParseResponseAsync(actual);
        var expectedResponse = ParseResponseAsync(expected);
        return new ComparableObject(actualResponse.Result, expectedResponse.Result);
    }

    public async Task<JObject> ParseResponseAsync(Task<HttpResponseMessage> httpResponseMessage)
    {
        var content = await httpResponseMessage.Result.Content.ReadAsStringAsync();
        var response = JObject.Parse(content);
        return response;
    }
}