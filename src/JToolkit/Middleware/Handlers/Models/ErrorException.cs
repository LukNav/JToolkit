namespace JToolkit.Middleware.Handlers.Models;

public abstract class ErrorException : Exception
{
    /// <summary>
    /// Unique, alphanumeric string, that represents human readable purpose of the numerical symbol
    /// </summary>
    /// <example>notFound</example>
    public string? Reason { get; }

    /// <summary>Domain specific information</summary>
    public IDictionary<string, object>? Params { get; }

    /// <summary>HTTP status code</summary>
    public abstract int StatusCode { get; }

    /// <inheritdoc />
    protected ErrorException(
        string? reason = null,
        string? message = null,
        Exception? innerException = null,
        IDictionary<string, object>? parameters = null)
        : base(message, innerException)
    {
        Reason = reason;
        if (parameters == null)
            return;
        Params = new Dictionary<string, object>(parameters);
    }
}