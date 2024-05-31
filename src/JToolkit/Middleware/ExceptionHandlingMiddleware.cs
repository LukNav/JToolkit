using System.Runtime.CompilerServices;
using JToolkit.Middleware.Extensions;
using JToolkit.Middleware.Models;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace JToolkit.Middleware // TODO: Separate exception handlers from wrapper.
{
  /// <summary>Handles all exceptions within the application</summary>
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    /// <summary>Middleware constructor</summary>
    /// <param name="next">Next request delegate</param>
    public ExceptionHandlingMiddleware(RequestDelegate next) => this._next = next ?? throw new ArgumentNullException(nameof (next));

    /// <summary>Invoke the middleware</summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>Next request</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        if (httpContext.Response.HasStarted)
          throw;
        httpContext.Response.Clear();
        ErrorResponse error = GetError(ex, httpContext);
        httpContext.Response.StatusCode = GetStatusCode(ex);
        httpContext.Response.ContentType = "application/json";
        httpContext.Items.Add("exception", ex);
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
      }
    }

    /// <summary>Create ErrorResponse from Exception</summary>
    /// <param name="exception">Exception</param>
    /// <param name="context">HTTP context</param>
    /// <returns>Error response</returns>
    protected virtual ErrorResponse GetError(Exception exception, HttpContext context)
    {
      string reason = exception.GetReason();
      ErrorResponse response;
      if (!string.IsNullOrEmpty(reason))
      {
        response = new ErrorResponse(reason, exception.Message);
        this.SetParams(response, exception);
      }
      else if (exception is ErrorException errorExceptionBase)
      {
        response = new ErrorResponse(errorExceptionBase.Reason, errorExceptionBase.Message, errorExceptionBase.Params);
        if (errorExceptionBase is ForbiddenException)
        {
          IHeaderDictionary headers = context.Response.Headers;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
          interpolatedStringHandler.AppendLiteral("Bearer error=\"");
          interpolatedStringHandler.AppendFormatted("insufficientAccess".CamelCaseToSnakeCase());
          interpolatedStringHandler.AppendLiteral("\", error_description=\"");
          interpolatedStringHandler.AppendFormatted(response.Message);
          interpolatedStringHandler.AppendLiteral("\"");
          StringValues stringAndClear = (StringValues) interpolatedStringHandler.ToStringAndClear();
          headers.Add("WWW-Authenticate", stringAndClear);
          response.Reason = "insufficientAccess";
          response.Message = exception.Message;
        }
      }
      else
      {
        response = new ErrorResponse("internalServerError", "Unexpected internal server error.");
        this.SetParams(response, exception);
      }
      return response;
    }

    private void SetParams(ErrorResponse response, Exception ex)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (KeyValuePair<string, object> keyValuePair in ex.GetParams())
        dictionary[keyValuePair.Key] = keyValuePair.Value;
      response.Params = dictionary.Count > 0 ? dictionary : null;
    }

    /// <summary>Get response status code from exception</summary>
    /// <param name="exception">Exception</param>
    /// <returns>Status code for response</returns>
    protected virtual int GetStatusCode(Exception exception)
    {
      int statusCode;
      switch (exception)
      {
        case FormatException _:
        case JsonException _:
          statusCode = 400;
          break;
        case TimeoutException _:
          statusCode = 408;
          break;
        case ErrorException errorExceptionBase:
          statusCode = errorExceptionBase.StatusCode;
          break;
        default:
          statusCode = 500;
          break;
      }
      return statusCode;
    }
  }
}
