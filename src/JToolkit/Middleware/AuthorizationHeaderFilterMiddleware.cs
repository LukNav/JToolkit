using Microsoft.Extensions.Primitives;

namespace JToolkit.Middleware
{
    public class AuthorizationHeaderFilterMiddleware(RequestDelegate next)
    {
        private const string AuthorizationHeaderKey = "Authorization";
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof (next));
        private const char Separator = ',';

        public async Task InvokeAsync(HttpContext httpContext)
        {
            StringValues source;
            httpContext.Request.Headers.TryGetValue(AuthorizationHeaderKey, out source);
            if (source.Any<string>() && !source.Contains(null))
            {
                string[] array = source.SelectMany<string, string>(v => v!.Split(new [] { Separator }, 
                    StringSplitOptions.RemoveEmptyEntries)).ToArray();
                if (array.Length > 1)
                {
                    string? str = array.FirstOrDefault((Func<string, bool>) (v => v.Trim().StartsWith("Bearer", StringComparison.InvariantCulture)));
                    if (str != null)
                        httpContext.Request.Headers[AuthorizationHeaderKey] = (StringValues) str.Trim();
                }
            }
            await _next(httpContext);
        }
    }
}