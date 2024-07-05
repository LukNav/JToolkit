namespace JToolkit.Http.Exceptions;

public class RequestTypeRequiredException : Exception
{
    public RequestTypeRequiredException(string message, string reason = "missingRequestType", IDictionary<string, object>? parameters = null)
    {
        Reason = reason;
        Message = message;
        Parameters = parameters;
    }

    public string Reason { get; }
    public override string Message { get; }
    public IDictionary<string, object>? Parameters { get; }
}