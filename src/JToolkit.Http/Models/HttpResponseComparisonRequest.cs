namespace JToolkit.Http.Models;

public class HttpResponseComparisonRequest
{
    public required HttpRequest Actual { get; set; }
    public required HttpRequest Expected { get; set; }
}