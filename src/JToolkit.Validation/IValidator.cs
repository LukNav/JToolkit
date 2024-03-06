namespace JToolkit.Validation;

public interface IValidator<in T>
{
    IAsyncEnumerable<ValidationFinding> ValidateAsync(T request, CancellationToken cancellationToken);
}