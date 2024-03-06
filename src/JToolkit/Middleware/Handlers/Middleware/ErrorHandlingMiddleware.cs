using System.Runtime.CompilerServices;
using System.Text.Json;
using JToolkit.Middleware.Handlers.Extensions;
using JToolkit.Middleware.Handlers.Models;
using Microsoft.Extensions.Primitives;

namespace JToolkit.Middleware.Handlers.Middleware
{
  /// <summary>Handles all HTTP errors within the application</summary>
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next ?? throw new ArgumentNullException(nameof (next));

    /// <summary>Invoke the middleware</summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>Next request</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
      await _next(httpContext);
      if (httpContext.Response.StatusCode < 400 || httpContext.Response.StatusCode >= 600)
        return;
      ErrorResponse responseFromCode = GetErrorResponseFromCode(httpContext.Response.StatusCode);
      if (httpContext.Response.HasStarted)
        return;
      int statusCode = httpContext.Response.StatusCode;
      StringValues stringValues = httpContext.Response.Headers["WWW-Authenticate"];
      httpContext.Response.Clear();
      httpContext.Response.ContentType = "application/json";
      httpContext.Response.StatusCode = statusCode;
      if (string.IsNullOrWhiteSpace(stringValues) || stringValues.ToString().Equals("Bearer", StringComparison.InvariantCultureIgnoreCase))
      {
        switch (httpContext.Response.StatusCode)
        {
          case 401:
            stringValues = BuildErrorResponse("todo", "invalidToken", "The access token is missing.");
            responseFromCode.Reason = "invalidToken";
            responseFromCode.Message = "The access token is missing.";
            break;
          case 403:
            stringValues = BuildErrorResponse("todo", "insufficientScope", "The access token does not contain scopes required to access the resource.");
            responseFromCode.Reason = "insufficientScope";
            responseFromCode.Message = "The access token does not contain scopes required to access the resource.";
            break;
        }
      }
      httpContext.Response.Headers.Add("WWW-Authenticate", stringValues);
      await httpContext.Response.WriteAsync(JsonSerializer.Serialize(responseFromCode));
    }

    private static StringValues BuildErrorResponse(string realm, string error, string errorDescription)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(71, 2);
      interpolatedStringHandler2.AppendLiteral("Bearer Realm=\"");
      interpolatedStringHandler2.AppendFormatted(realm);
      interpolatedStringHandler2.AppendLiteral("\" error=\"");
      interpolatedStringHandler2.AppendFormatted(error.CamelCaseToSnakeCase());
      interpolatedStringHandler2.AppendLiteral("\", error_description=\"");
      interpolatedStringHandler2.AppendFormatted(errorDescription);
      interpolatedStringHandler2.AppendLiteral("\"");
      return (StringValues)interpolatedStringHandler2.ToStringAndClear();
    }
   
    /// <summary>Returns the error response based on the status code</summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <returns>Error response</returns>
    private static ErrorResponse GetErrorResponseFromCode(int statusCode)
    {
      ErrorResponse responseFromCode;
      switch (statusCode)
      {
        case 400:
          responseFromCode = new ErrorResponse("validationFailed", "The request is invalid.");
          break;
        case 401:
          responseFromCode = new ErrorResponse("authenticationFailed", "The access token is missing, invalid, or is revoked.");
          break;
        case 403:
          responseFromCode = new ErrorResponse("authorizationFailed", "The subject of the token does not have the permissions that are required to access the resource.");
          break;
        case 404:
          responseFromCode = new ErrorResponse("notFound", "Resource not found.");
          break;
        case 405:
          responseFromCode = new ErrorResponse("methodNotAllowed", "The requested method is not supported for resource.");
          break;
        case 406:
          responseFromCode = new ErrorResponse("notAcceptable", "The requested representation is not supported.");
          break;
        case 408:
          responseFromCode = new ErrorResponse("requestTimeout", "The request has timed-out.");
          break;
        case 413:
          responseFromCode = new ErrorResponse("requestTooLarge", "The request is too large.");
          break;
        case 415:
          responseFromCode = new ErrorResponse("unsupportedMediaType", "The provided payload is not supported.");
          break;
        case 503:
          responseFromCode = new ErrorResponse("serviceUnavailable", "The service is currently not available.");
          break;
        default:
          responseFromCode = new ErrorResponse("internalServerError", "Unexpected internal server error.");
          break;
      }
      return responseFromCode;
    }
  }
}
