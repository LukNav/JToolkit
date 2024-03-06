using JToolkit.Comparer.Models;
using Newtonsoft.Json.Linq;

namespace JToolkit.Comparer.Comparer;

public class JsonComparer : IJsonComparer
{
    public ComparisonResult Compare(ComparableObject comparableObj)
    {
        var differences = CompareObjects(comparableObj.Actual, comparableObj.Expected);
        
        if (differences.Count() > 0)
            return new ComparisonResult(false, differences);
        
        //fallback sanity check
        if (!JToken.DeepEquals(comparableObj.Actual, comparableObj.Expected))
            return new ComparisonResult(false, null);

        return new ComparisonResult(true, null);
    }
    
      /// <summary>
    /// Deep compare two NewtonSoft JObjects. If they don't match, returns text diffs
    /// </summary>
    /// <param name="Actual">The expected results</param>
    /// <param name="Expected">The actual results</param>
    /// <returns>Differences</returns>

    private static List<string> CompareObjects(JObject Actual, JObject Expected)
    {
        List<string> differences = new List<string>();
        foreach (KeyValuePair<string, JToken> sourcePair in Actual)
        {
            if (sourcePair.Value.Type == JTokenType.Object)
            {
                if (Expected.GetValue(sourcePair.Key) == null)
                {
                    differences.Add("Key " + sourcePair.Key
                                              + " not found" + Environment.NewLine);
                }
                else if (Expected.GetValue(sourcePair.Key).Type != JTokenType.Object) {
                    differences.Add("Key " + sourcePair.Key
                                              + " is not an object in target" + Environment.NewLine);
                }                    
                else
                {
                    differences.AddRange(CompareObjects(sourcePair.Value.ToObject<JObject>(),
                        Expected.GetValue(sourcePair.Key).ToObject<JObject>()));
                }
            }
            else if (sourcePair.Value.Type == JTokenType.Array)
            {
                if (Expected.GetValue(sourcePair.Key) == null)
                {
                    differences.Add("Key " + sourcePair.Key
                                              + " not found" + Environment.NewLine);
                }
                else
                {
                    differences.AddRange(CompareArrays(sourcePair.Value.ToObject<JArray>(),
                        Expected.GetValue(sourcePair.Key).ToObject<JArray>(), sourcePair.Key));
                }
            }
            else
            {
                JToken expected = sourcePair.Value;
                var actual = Expected.SelectToken(sourcePair.Key);
                if (actual == null)
                {
                    differences.Add("Key " + sourcePair.Key
                                        + " not found" + Environment.NewLine);
                }
                else
                {
                    if (!JToken.DeepEquals(expected, actual))
                    {
                        differences.Add("Key " + sourcePair.Key + ": "
                                            + sourcePair.Value + " !=  "
                                            + Expected.Property(sourcePair.Key).Value
                                            + Environment.NewLine);
                    }
                }
            }
        }
        return differences;
    }


      /// <summary>
      /// Deep compare two NewtonSoft JArrays. If they don't match, returns text diffs
      /// </summary>
      /// <param name="source">The expected results</param>
      /// <param name="target">The actual results</param>
      /// <param name="arrayName">The name of the array to use in the text diff</param>
      /// <returns>Text string</returns>

      private static List<string> CompareArrays(JArray source, JArray target, string arrayName)
      {
          List<string> differences = new List<string>();
          
          var actualArray = source.Where(s=>s.Type == JTokenType.String).Select(x => x.Value<string>()).ToArray();
          var expectedArray = target.Where(s=>s.Type == JTokenType.String).Select(x => x.Value<string>()).ToArray();
          var newValues = actualArray.Where(x => !expectedArray.Contains(x));
          var missingValues = expectedArray.Where(x => !actualArray.Contains(x));
          
          if (missingValues.Any())
          {
              differences.Add($"Column [{arrayName}] Missing Values: {String.Join(", ", missingValues)}");
          }
          if (newValues.Any())
          {
              differences.Add($"Column [{arrayName}] New values: {String.Join(", ", newValues)}");
          }
          //
          //
          // for (int index = 0; index < source.Count(); index++)
          // {
          //     var expected = source[index];
          //     if (expected.Type == JTokenType.Object)
          //     {
          //         var actual = (index >= target.Count()) ? new JObject() : target[index];
          //         differences.AddRange(CompareObjects(expected.ToObject<JObject>(),
          //             actual.ToObject<JObject>()));
          //     }
          //     if (expected.Type == JTokenType.Array)
          //     {
          //         var actual = (index >= target.Count()) ? new JObject() : target[index];
          //         differences.AddRange(CompareArrays(actual.ToObject<JArray>(), expected.ToObject<JArray>(), $"{arrayName}[{index}]"));
          //     }
          // }
          return differences;
      }
      
       //
    // /// <summary>
    // /// Deep compare two NewtonSoft JArrays. If they don't match, returns text diffs
    // /// </summary>
    // /// <param name="source">The expected results</param>
    // /// <param name="target">The actual results</param>
    // /// <param name="arrayName">The name of the array to use in the text diff</param>
    // /// <returns>Text string</returns>
    //
    // private static List<string> CompareArrays(JArray source, JArray target, string arrayName = "")
    // {
    //     List<string> differences = new List<string>();
    //     var actualArray = source.OrderBy(x => x.Value<string>()).ToArray();
    //     var expectedArray = target.OrderBy(x => x.Value<string>()).ToArray();
    //     for (int index = 0; index < actualArray.Count(); index++)
    //     {
    //
    //         var expected = actualArray[index];
    //         if (expected.Type == JTokenType.Object)
    //         {
    //             var actual = (index >= expectedArray.Count()) ? new JObject() : expectedArray[index];
    //             differences.AddRange(CompareObjects(expected.ToObject<JObject>(),
    //                 actual.ToObject<JObject>()));
    //         }
    //         else
    //         {
    //
    //             var actual = (index >= expectedArray.Count()) ? "" : expectedArray[index];
    //             if (!JToken.DeepEquals(expected, actual))
    //             {
    //                 if (String.IsNullOrEmpty(arrayName))
    //                 {
    //                     differences.Add("Index " + index + ": " + expected
    //                                        + " != " + actual + Environment.NewLine);
    //                 }
    //                 else
    //                 {
    //                     differences.Add("Key " + arrayName
    //                                               + "[" + index + "]: " + expected
    //                                               + " != " + actual + Environment.NewLine);
    //                 }
    //             }
    //         }
    //     }
    //     return differences;
    // }
}