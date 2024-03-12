// using System.Text.Json;
// using System.Text.Json.Nodes;
// using JToolkit.Playground.JsonTools;
// using NUnit.Framework;
//
// namespace JToolkit.Playground;
//
// public class JsonToolkit
// {
//     [Test]
//     public void DistinctRowsFromJsonFile()
//     {
//         string jsonFilePath = "C:\\Work\\adform-logs-live\\Adform.Logs.Live\\bin\\Debug\\!_result.txt";
//         string distinctByColumn= "Url";
//
//         string jsonFile = File.ReadAllText(jsonFilePath);
//         var jArray = JsonNode.Parse(jsonFile)?.AsArray();
//         var distincts = jArray?.Select(j=>j?[distinctByColumn]?.ToString()).Distinct().ToList();
//         
//         distincts?.ForEach(d=>Console.WriteLine(d));
//     }
//     
//     [Test]
//     public void FindAndReplaceRowsInJsonFile()
//     {
//         string jsonFilePath = "C:\\Work\\adform-logs-live\\Adform.Logs.Live\\bin\\Debug\\!_result.txt";
//         string column = "Url"; // Json object title in question
//         Dictionary<string, string> replaceMap = new()
//         {
//             { "einstein-data.einstein.svc/", "prokube-dk1.adform.zone:22224/" },
//             { "einstein-data/", "prokube-dk1.adform.zone:22224/" },
//             { "inlb.app.adform.com:20097/", "prokube-dk1.adform.zone:20098/" }
//         };
//
//         string jsonFile = File.ReadAllText(jsonFilePath);
//         JsonArray? jArray = JsonNode.Parse(jsonFile)?.AsArray();
//         
//         bool? replacedAnyRows = jArray?.TryReplaceRows(column, replaceMap);
//         
//         Assert.That(replacedAnyRows, Is.True, "No rows were replaced. Target file not overwritten");
//         
//         File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jArray));
//         
//         Console.WriteLine("Some rows were replaced. Target file overwritten");
//     }
// }