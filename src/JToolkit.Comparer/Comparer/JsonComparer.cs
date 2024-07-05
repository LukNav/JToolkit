using JsonDiffer;
using JToolkit.Comparer.Models;

namespace JToolkit.Comparer.Comparer;

public class JsonComparer : IJsonComparer
{
    public ComparisonResult Compare(ComparableObject request)
    {
        JsonDifferentiator differ = new JsonDifferentiator(OutputMode.Detailed, false);
        var result = differ.Differentiate(request.Expected, request.Actual);
        return new ComparisonResult(result);
    }
    
    // public ComparisonResult Compare(ComparableObject comparableObj)
    // {
    //     //Ordering is done on mapping layer
    //     //Reader reads -> Convert to custom type (Self ordering by keys) -> Deep Compare
    //     // >> Create custom value type mapper
    //     //Deep Compare: Iterate through objects: Compare type -> compare value type
    //     //Compare arrays not working :)
    //
    //     var differences = CompareObjects(comparableObj.Actual, comparableObj.Expected);
    //
    //     if (differences.Count() > 0)
    //     {
    //         return new ComparisonResult(false, differences);
    //     }
    //     
    //     //fallback sanity check
    //     // if (!JToken.DeepEquals(comparableObj.Actual, comparableObj.Expected)) // TODO: Fix fallback unordered values issue
    //     // {
    //     //     // TODO: Log inner uncaught exception
    //     //     return new ComparisonResult(false, null);
    //     // }
    //     return new ComparisonResult(true, null);
    // }

    // /// <summary>
    // /// Deep compare two NewtonSoft JObjects. If they don't match, returns text diffs
    // /// </summary>
    // /// <param name="actual">The expected results</param>
    // /// <param name="expected">The actual results</param>
    // /// <param name="differences">Diff passthrough</param>
    // /// <returns>Differences</returns>
    // private static List<Difference> CompareObjects(JObject actual, JObject expected, List<Difference>? differences = null)
    // {
    //     if (differences == null)
    //     {
    //         differences = new List<Difference>();
    //     }
    //
    //     foreach (KeyValuePair<string, JToken?> actualKeyVal in actual)
    //     {
    //         if (actualKeyVal.Value?.Type == JTokenType.Object)
    //         {
    //             CompareObject(expected, actualKeyVal, differences);
    //         }
    //         else if (actualKeyVal.Value?.Type == JTokenType.Array)
    //         {
    //             CompareArray(expected, actualKeyVal, differences);
    //         }
    //         else
    //         {
    //             CompareValue(expected, actualKeyVal, differences);
    //         }
    //     }
    //
    //     return differences;
    // }
    //
    // private static void CompareValue(JObject expected, KeyValuePair<string, JToken> actual, 
    //     List<Difference> differences)
    // {
    //     JToken expectedValue = actual.Value;
    //     var actualValue = expected.SelectToken(actual.Key);
    //     if (actualValue == null)
    //     {
    //         differences.Add(CreateNotFoundDifference(actual));
    //     }
    //     else
    //     {
    //         if (!JToken.DeepEquals(expectedValue, actualValue))
    //         {
    //             differences.Add(new Difference
    //             {
    //                 Key = actual.Key,
    //                 Reason = DifferenceReason.ValuesMismatch,
    //                 Actual = actual.Value
    //             });
    //         }
    //     }
    // }
    //
    // // private static void CompareValue(JToken actual, JToken expected,
    // //     List<Difference> differences)
    // // {
    // //     if (!JToken.DeepEquals(actual, expected))
    // //     {
    // //         differences.Add(new Difference // TODO: test the scenario
    // //         {
    // //             Key = actual.Parent?.Path ?? "",
    // //             Reason = DifferenceReason.ValuesMismatch,
    // //             Actual = actual,
    // //             Expected = expected
    // //         });
    // //     }
    // // }
    //
    // private static void CompareArray(JObject expected, KeyValuePair<string, JToken> sourcePair,
    //     List<Difference> differences)
    // {
    //     var expectedValue = expected.GetValue(sourcePair.Key);
    //     if (expectedValue == null)
    //     {
    //         differences.Add(CreateNotFoundDifference(sourcePair)); // TODO: Cover scenario in tests. Potential issue when comparing null with array? Might be solved with naming
    //     }
    //     else if(expectedValue.Type != JTokenType.Array)
    //     {
    //         differences.Add(new Difference
    //         {
    //             Key = sourcePair.Key,
    //             Reason = DifferenceReason.TypesMismatch,
    //             Actual = sourcePair.Value,
    //             Expected = expected.GetValue(sourcePair.Key)
    //         }); // TODO: Cover scenario in tests. Might be an issue when the type is null?
    //     }
    //     else
    //     {
    //         var actualVal = sourcePair.Value.ToObject<JArray>()!; // TODO: Solve nullability suppresor (potentially it is fine, since type is checked to be Array)
    //         var expectedVal = expectedValue.ToObject<JArray>()!;
    //         var comparisonResult = CompareArrays(actualVal, expectedVal, sourcePair.Key);
    //         differences.AddRange(comparisonResult);
    //     }
    // }
    //
    // private static void CompareObject(JObject Expected, KeyValuePair<string, JToken> sourcePair,
    //     List<Difference> differences)
    // {
    //     var expectedValue = Expected[sourcePair.Key];
    //     if (expectedValue == null)
    //     {
    //         differences.Add(CreateNotFoundDifference(sourcePair));
    //     }
    //     else if (expectedValue.Type != JTokenType.Object)
    //     {
    //         differences.Add(new Difference
    //         {
    //             Key = sourcePair.Key,
    //             Reason = DifferenceReason.TypesMismatch,
    //             Expected = expectedValue,
    //             Actual = sourcePair.Value
    //         });
    //     }
    //     else
    //     {
    //         var actual = sourcePair.Value.ToObject<JObject>() ?? throw new InvalidOperationException();
    //         var expected = expectedValue.ToObject<JObject>() ?? throw new InvalidOperationException();
    //         differences.AddRange(CompareObjects(actual, expected, differences));
    //     }
    // }
    //
    // private static Difference CreateNotFoundDifference(KeyValuePair<string, JToken> sourcePair)
    // {
    //     return new Difference
    //     {
    //         Key = sourcePair.Key,
    //         Reason = DifferenceReason.MissingValues,
    //         Expected = "",
    //         Actual = sourcePair.Value
    //     };
    // }
    //
    // /// <summary>
    // /// Deep compare two NewtonSoft JArrays. If they don't match, returns text diffs
    // /// It expects ordered arrays
    // /// </summary>
    // /// <param name="actual">The actual results</param>
    // /// <param name="expected">The expected results</param>
    // /// <param name="arrayName">The name of the array to use in the text diff</param>
    // /// <returns>Text string</returns>
    //
    // private static List<Difference> CompareArrays(JArray actualArr, JArray expectedArr, string arrayName)
    // {
    //     List<Difference> differences = new List<Difference>();
    //    
    //     for (int index = 0; index < expectedArr.Count(); index++)
    //     {
    //         var expected = expectedArr[index];
    //         if (index >= actualArr.Count)
    //         {
    //             differences.Add(new Difference
    //             {
    //                 Key = $"{arrayName}[{index}]",
    //                 Reason = DifferenceReason.MissingValues,
    //                 Expected = expected
    //             });
    //             continue;
    //         }
    //         
    //         var actual = actualArr[index]; // TODO: If null - add difference
    //         
    //         if (expected.Type == JTokenType.Array)
    //         {
    //             differences.AddRange(CompareArrays(actual.ToObject<JArray>(), expected.ToObject<JArray>(), $"{arrayName}[{index}]"));
    //         }
    //         else if (expected.Type == JTokenType.Object)
    //         {
    //             CompareObjects(actual.ToObject<JObject>(), expected.ToObject<JObject>(), differences);
    //         }
    //         else
    //         {
    //             CompareValue(actual, expected, differences);
    //         }
    //     }
    //
    //     return differences;
    // }
}