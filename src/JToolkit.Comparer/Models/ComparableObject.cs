using Newtonsoft.Json.Linq;

namespace JToolkit.Comparer.Models;

public record ComparableObject(JObject Actual, JObject Expected);
