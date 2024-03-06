using JToolkit.Contracts;
using JToolkit.Validation;
using JToolkit.Validation.ValidationRules.StatsRequest;
using JToolkit.Validation.Validators;

namespace JToolkit.Capabilities;

public static class StartupValidation
{
    public static void ConfigureValidationRules(this IServiceCollection services)
    {
        services.AddSingleton<PlaceholderValidationRule>(); // TODO: Actual validations

        services.AddSingleton<IList<IValidationRule<PlaceholderRequest>>>(s => 
            new List<IValidationRule<PlaceholderRequest>>
            {
                s.GetRequiredService<PlaceholderValidationRule>()
            });

        services.AddSingleton<IValidator<PlaceholderRequest>, RequestValidator<PlaceholderRequest>>();
    }
}