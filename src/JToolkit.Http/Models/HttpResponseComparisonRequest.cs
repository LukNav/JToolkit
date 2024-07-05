namespace JToolkit.Http.Models;

public class HttpResponseComparisonRequest
{
    public required HttpRequest ActualRequest { get; set; }
    public required HttpRequest ExpectedRequest { get; set; }
    public required HttpRequestFields? CommonRequestFields { get; set; }
}