using System.Runtime.CompilerServices;
using JToolkit.Contracts;

namespace JToolkit.Validation.ValidationRules.StatsRequest;

public class PlaceholderValidationRule : IValidationRule<PlaceholderRequest>
{
    public async IAsyncEnumerable<ValidationFinding> ValidateAsync(PlaceholderRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        int holdor = request.Holdor;

        if (holdor == 0 || holdor > 999)
        {
            yield return await Task.FromResult(CreateInvalidDateRange());
        }
    }

    private static ValidationFinding CreateInvalidDateRange()
    {
        return new ValidationFinding
        {
            Reason = "invalidHoldor",
            Message = "Not holdor or holdor is greater than 999."
        };
    }
}

