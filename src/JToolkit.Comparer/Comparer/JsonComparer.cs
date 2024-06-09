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
                var expectedValue = expected[actualKeyVal.Key];
                if (expectedValue == null)
                {
                    differences.Add(CreateNotFoundDifference(actualKeyVal));
                }
                else if (expectedValue.Type != JTokenType.Object)
                {
                    differences.Add(new Difference
                    {
                        Key = actualKeyVal.Key,
                        Reason = DifferenceReason.TypesMismatch,
                        Expected = expectedValue,
                        Actual = actualKeyVal.Value
                    });
                }
                else // If both are objects, compare again
                {
                    var actualObj = actualKeyVal.Value.ToObject<JObject>() ?? throw new InvalidOperationException();
                    var expectedObj = expectedValue.ToObject<JObject>() ?? throw new InvalidOperationException();
                    CompareObjects(actualObj, expectedObj, differences);
                }
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
        
        for (int i = 0; i < actual.Count; i++)
        {
            var actualItem = actual[i];
            bool isFound = false;
            for (int j = 0; j < expected.Count; j++)
            {
                var expectedItem = expected[j];
                if (actualItem.Type == expectedItem.Type)
                {
                    if (actualItem.Type == JTokenType.Object)
                    {
                        var actualObj = actualItem.ToObject<JObject>() ?? throw new InvalidOperationException();
                        var expectedObj = expectedItem.ToObject<JObject>() ?? throw new InvalidOperationException();

                        var comparisonResult = CompareObjects(actualObj, expectedObj);
                        if (comparisonResult.Count == 0)
                        {
                            isFound = true;
                            break;
                        }
                    }
                }
            }
            if (!isFound)
            {
                differences.Add(new Difference()
                {
                    Actual = actualItem,
                    Key = arrayName,
                    Reason = DifferenceReason.MissingValues
                });
            }
                
        }
        
        // var newValues = actual.Where(x => !expected.Contains(x)); // TODO: Test string array, but in actual one string is object with key/value same as string name
        // var missingValues = expected.Where(x => !actual.Contains(x));
        //
        // if (missingValues.Any())
        // {
        //     differences.Add(new Difference
        //     {
        //         Key = arrayName,
        //         Reason = DifferenceReason.MissingValues,
        //         Expected = missingValues
        //     });
        // }
        //
        // if (newValues.Any())
        // {
        //     differences.Add(new Difference
        //     {
        //         Key = arrayName,
        //         Reason = DifferenceReason.ExtraValues,
        //         Actual = newValues
        //     });
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