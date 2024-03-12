// using JToolkit.Comparer.Exceptions;
// using JToolkit.Comparer.Models;
// using Newtonsoft.Json.Linq;
//
// namespace JToolkit.Comparer.Mappers;
//
// public class ComparableValue
// {
//     
// }
//
// public record ComparableObject(string Key, ComparableValue? Value);
//
// public record ComparableObjects(ComparableObject? Expected, ComparableObject? Actual);
//
// public interface IComparisonMapperV2
// {
//     ComparableObjects Map(ComparisonRequest request);
// }
//
// public class ComparisonMapperV2 : IComparisonMapperV2
// {
//     public ComparableObjects Map(ComparisonRequest request)
//     {
//         if(request.Actual is null || request.Expected is null)
//             throw new InvalidJsonComparisonRequestException("Invalid Request", "ActulJson and ExpectedJson fields are required."); // TODO: Exception middleware should handle this
//
//         var actual = JObject.Parse(request.Actual.ToString()!); // TODO: Fix suppresion ('!') operator. Decide if the mapper could map to custom object, so we could compare ANYTHING, not only json
//         var expected = JObject.Parse(request.Expected.ToString()!);
//         return new ComparableObjects(actual, expected);
//     }
// }