namespace JToolkit.Validation.Exceptions;

[Serializable]
public class RequestValidationException : Exception
{
    public string Reason { get; }
    public string MessageText { get; } // TODO: Rename to Message, override it.
    public IDictionary<string, object>? Parameters { get; }

    public RequestValidationException(
        string? reason,
        string? message,
        IDictionary<string, object>? parameters = null) : base( message)
    {
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        MessageText = message ?? throw new ArgumentNullException(nameof(message));
        Parameters = parameters;
    }
}
