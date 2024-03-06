using JToolkit.Middleware;

namespace JToolkit.Capabilities;

public static class StartupErrorHandler
{
    public static void UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}