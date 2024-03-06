using JToolkit.Comparer.Exceptions;
using JToolkit.Middleware.Exceptions;
using JToolkit.Validation.Exceptions;

namespace JToolkit.Middleware;

public class ExceptionWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionWrapperMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next)); // TODO: Convert to primary constructor
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (RequestValidationException ex)
        {
            throw new BadRequestException(innerException: ex, message: ex.MessageText,
                reason: ex.Reason, parameters: ex.Parameters);
        }
        catch (InvalidJsonComparisonRequestException ex)
        {
            throw new BadRequestException(innerException: ex, message: ex.Message,
                reason: ex.Reason, parameters: ex.Parameters);
        }
        catch (Exception ex)
        {
            throw new InternalServerErrorException(innerException: ex, message: ex.Message);
        }
    }
}