using System.Runtime.CompilerServices;

namespace JToolkit.Validation.Validators;

public class RequestValidator<T> : IValidator<T>
{
    private readonly IList<IValidationRule<T>> _validationRules;

    public RequestValidator(IList<IValidationRule<T>> validationRules)
    {
        _validationRules = validationRules ?? throw new ArgumentNullException(nameof(validationRules));
    }

    public async IAsyncEnumerable<ValidationFinding> ValidateAsync(T request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var validationTasks = _validationRules.Select(r => r.ValidateAsync(request, cancellationToken));

        foreach (IAsyncEnumerable<ValidationFinding> validationFindings in validationTasks)
        {
            await foreach (ValidationFinding validationFinding in validationFindings.WithCancellation(cancellationToken))
            {
                yield return validationFinding;
            }
        }
    }
}


