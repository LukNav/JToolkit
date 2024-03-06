using System.Text.Json.Nodes;

namespace JToolkit.Playground.JsonTools;

public static class JsonExtensions // TODO: Move this. Should be moved to JTools namespace
{
    /// <summary>
    /// Selects column in JsonArray, gets column values, then replaces substring in each value
    /// </summary>
    /// <param name="column">Row column name</param>
    /// <param name="replaceMap">Dictionary maps old string keys with keys: Dictionary OldKey:NewKey</param>
    /// <param name="jArray">Json objects(columns) array</param>
    /// <returns>True if at least one value was replaced</returns>
    public static bool TryReplaceRows(this JsonArray jArray, string column, IDictionary<string, string> replaceMap)
    {
        bool anyReplaced = false;
        foreach (JsonNode? jNode in jArray)
        {
            if (jNode == null)
            {
                continue;
            }

            var rowReplaced = replaceMap
                .ToList()
                .Any(rm=>TryReplaceRow(jNode, column, rm.Key, rm.Value));
            anyReplaced = anyReplaced || rowReplaced;
        }

        return anyReplaced;
    }
    
    
    public static bool TryReplaceRow(this JsonNode jNode, string column, string replaceKey, string replaceValue)
    {
        var row = jNode[column]?.ToString();
        if (row != null && row.Contains(replaceKey))
        {
            jNode[column] = jNode[column]?.ToString().Replace(replaceKey, replaceValue);
            return true;
        }

        return false;
    }
}