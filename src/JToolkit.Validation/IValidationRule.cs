namespace JToolkit.Validation;

public interface IValidationRule<in T>
{
    IAsyncEnumerable<ValidationFinding> ValidateAsync(T request, CancellationToken cancellationToken);
}

public class ValidationFinding
{
    public string? Reason { get; set; }
    public string? Message { get; set; }
}

