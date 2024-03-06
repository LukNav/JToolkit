namespace JToolkit.Middleware.Handlers.Extensions;

public static class ExceptionExtensions
{

  /// <summary>Gets all domain specific information</summary>
  /// <param name="ex">Exception</param>
  /// <returns>Key</returns>
  public static IEnumerable<KeyValuePair<string, object>> GetParams(this Exception ex)
  {
    IEnumerable<KeyValuePair<string, object>>? keyValuePairs = null;
    if (ex.Data.Contains("params"))
      keyValuePairs = (IEnumerable<KeyValuePair<string, object>>?)ex.Data["params"];
    return keyValuePairs ?? Enumerable.Empty<KeyValuePair<string, object>>();
  }

  /// <summary>Gets reason message in camelCase format</summary>
  /// <param name="ex">Exception</param>
  /// <returns>String</returns>
  public static string GetReason(this Exception ex) => ex.Data["reason"]?.ToString() ?? string.Empty;
}