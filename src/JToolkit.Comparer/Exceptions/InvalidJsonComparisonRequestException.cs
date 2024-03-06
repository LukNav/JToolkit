namespace JToolkit.Comparer.Exceptions;

public class InvalidJsonComparisonRequestException : Exception
{
    public InvalidJsonComparisonRequestException(string reason, string message, IDictionary<string, object>? parameters = null)
    {
        Reason = reason;
        Message = message;
        Parameters = parameters;
    }

    public string Reason { get; }
    public override string Message { get; }
    public IDictionary<string, object>? Parameters { get; }
}