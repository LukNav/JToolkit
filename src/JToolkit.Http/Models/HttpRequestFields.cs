
namespace JToolkit.Http.Models;

public class HttpRequestFields
{
    public IReadOnlyDictionary<string,string>? Headers { get; set; }
    public RequestType? Method { get; set; }
    public object? Body { get; set; }
}