namespace JToolkit.Http.Models;

public class HttpRequest
{
    public required string Url { get; set; }
    public IReadOnlyDictionary<string,string>? Headers { get; set; }
    public required RequestType Method { get; set; }
    public object? Body { get; set; }
}