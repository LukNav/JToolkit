namespace JToolkit.Http.Exceptions;

public class UnsupportedRequestTypeException : Exception
{
    public UnsupportedRequestTypeException(string message, string reason = "unsupportedRequestType", IDictionary<string, object>? parameters = null)
    {
        Reason = reason;
        Message = message;
        Parameters = parameters;
    }

    public string Reason { get; }
    public override string Message { get; }
    public IDictionary<string, object>? Parameters { get; }
}