using JToolkit.Comparer.Models;
using Newtonsoft.Json.Linq;

namespace JToolkit.Comparer.Comparer;

public class JsonComparer : IJsonComparer
{
    public ComparisonResult Compare(ComparableObject comparableObj)
    {
        //Ordering is done on mapping layer
        //Reader reads -> Convert to custom type (Self ordering by keys) -> Deep Compare
        // >> Create custom value type mapper
        //Deep Compare: Iterate through objects: Compare type -> compare value type
        //Compare arrays not working :)

        var differences = CompareObjects(comparableObj.Actual, comparableObj.Expected);

        if (differences.Count() > 0)
        {
            return new ComparisonResult(false, differences);
        }
        
        //fallback sanity check
        // if (!JToken.DeepEquals(comparableObj.Actual, comparableObj.Expected)) // TODO: Fix fallback unordered values issue
        // {
        //     // TODO: Log inner uncaught exception
        //     return new ComparisonResult(false, null);
        // }
        return new ComparisonResult(true, null);
    }

    /// <summary>
    /// Deep compare two NewtonSoft JObjects. If they don't match, returns text diffs
    /// </summary>
    /// <param name="actual">The expected results</param>
    /// <param name="expected">The actual results</param>
    /// <param name="differences">Diff passthrough</param>
    /// <returns>Differences</returns>
    private static List<Difference> CompareObjects(JObject actual, JObject expected, List<Difference>? differences = null)
    {
        if (differences == null)
        {
            differences = new List<Difference>();
        }

        foreach (KeyValuePair<string, JToken?> actualKeyVal in actual)
        {
            if (actualKeyVal.Value?.Type == JTokenType.Object)
            {
                CompareObject(expected, actualKeyVal, differences);
            }
            else if (actualKeyVal.Value?.Type == JTokenType.Array)
            {
                CompareArray(expected, actualKeyVal, differences);
            }
            else
            {
                CompareValue(expected, actualKeyVal, differences);
            }
        }

        return differences;
    }

    private static void CompareValue(JObject expected, KeyValuePair<string, JToken> sourcePair, 
        List<Difference> differences)
    {
        JToken expectedValue = sourcePair.Value;
        var actual = expected.SelectToken(sourcePair.Key);// TODO: !!! Actual/Expected confusion?
        if (actual == null)
        {
            differences.Add(CreateNotFoundDifference(sourcePair));
        }
        else
        {
            if (!JToken.DeepEquals(expectedValue, actual))
            {
                differences.Add(new Difference
                {
                    Key = sourcePair.Key,
                    Reason = DifferenceReason.ValuesMismatch,
                    Actual = sourcePair.Value
                });
            }
        }
    }

    private static void CompareArray(JObject expected, KeyValuePair<string, JToken> sourcePair,
        List<Difference> differences)
    {
        var expectedValue = expected.GetValue(sourcePair.Key);
        if (expectedValue == null)
        {
            differences.Add(CreateNotFoundDifference(sourcePair)); // TODO: Cover scenario in tests. Potential issue when comparing null with array? Might be solved with naming
        }
        else if(expectedValue.Type != JTokenType.Array)
        {
            // TODO: Environment.NewLine shouldn't be here. Move to some aggregator
            differences.Add(new Difference
            {
                Key = sourcePair.Key,
                Reason = DifferenceReason.TypesMismatch,
                Actual = sourcePair.Value,
                Expected = expected.GetValue(sourcePair.Key)
            }); // TODO: Cover scenario in tests. Might be an issue when the type is null?
        }
        else
        {
            var actualVal = sourcePair.Value.ToObject<JArray>()!; // TODO: Solve nullability suppresor (potentially it is fine, since type is checked to be Array)
            var expectedVal = expectedValue.ToObject<JArray>()!;
            var comparisonResult = CompareArrays(actualVal, expectedVal, sourcePair.Key);
            differences.AddRange(comparisonResult);
        }
    }

    private static void CompareObject(JObject Expected, KeyValuePair<string, JToken> sourcePair,
        List<Difference> differences)
    {
        var expectedValue = Expected[sourcePair.Key];
        if (expectedValue == null)
        {
            differences.Add(CreateNotFoundDifference(sourcePair));
        }
        else if (expectedValue.Type != JTokenType.Object)
        {
            differences.Add(new Difference
            {
                Key = sourcePair.Key,
                Reason = DifferenceReason.TypesMismatch,
                Expected = expectedValue,
                Actual = sourcePair.Value
            });
        }
        else
        {
            var actual = sourcePair.Value.ToObject<JObject>() ?? throw new InvalidOperationException();
            var expected = expectedValue.ToObject<JObject>() ?? throw new InvalidOperationException();
            differences.AddRange(CompareObjects(actual, expected, differences));
        }
    }

    private static Difference CreateNotFoundDifference(KeyValuePair<string, JToken> sourcePair)
    {
        return new Difference
        {
            Key = sourcePair.Key,
            Reason = DifferenceReason.MissingValues,
            Expected = "",
            Actual = sourcePair.Value
        };
    }


    /// <summary>
    /// Deep compare two NewtonSoft JArrays. If they don't match, returns text diffs
    /// </summary>
    /// <param name="actual">The actual results</param>
    /// <param name="expected">The expected results</param>
    /// <param name="arrayName">The name of the array to use in the text diff</param>
    /// <returns>Text string</returns>

    private static List<Difference> CompareArrays(JArray actual, JArray expected, string arrayName)
    {
        List<Difference> differences = new List<Difference>();

        // TODO: Check if type is object type
        
        
        // ELSE - convert values to strings, compare strings
        var actualArray = actual.Select(x => x.Value<JToken>()).ToArray();
        var expectedArray = expected.Select(x => x.Value<JToken>()).ToArray();
        var newValues = actualArray.Where(x => !expectedArray.Contains(x));
        var missingValues = expectedArray.Where(x => !actualArray.Contains(x));

        if (missingValues.Any())
        {
            differences.Add(new Difference
            {
                Key = arrayName,
                Reason = DifferenceReason.MissingValues,
                Expected = missingValues
            });
        }

        if (newValues.Any())
        {
            differences.Add(new Difference
            {
                Key = arrayName,
                Reason = DifferenceReason.ExtraValues,
                Actual = newValues
            });
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