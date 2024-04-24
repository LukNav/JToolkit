using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using ExcelDataReader;
using JToolkit.Playground.JsonTools;
using NUnit.Framework;

namespace JToolkit.Playground;

public class JsonToolkit
{
    [Test]
    public void DistinctRowsFromJsonFile()
    {
        string jsonFilePath = "C:\\Work\\adform-logs-live\\Adform.Logs.Live\\bin\\Debug\\!_result.txt";
        string distinctByColumn= "Url";

        string jsonFile = File.ReadAllText(jsonFilePath);
        var jArray = JsonNode.Parse(jsonFile)?.AsArray();
        var distincts = jArray?.Select(j=>j?[distinctByColumn]?.ToString()).Distinct().ToList();
        
        distincts?.ForEach(d=>Console.WriteLine(d));
    }
    
    [Test]
    public void FindAndReplaceRowsInJsonFile()
    {
        string jsonFilePath = "C:\\Work\\adform-logs-live\\Adform.Logs.Live\\bin\\Debug\\!_result.txt";
        string column = "Url"; // Json object title in question
        Dictionary<string, string> replaceMap = new()
        {
            { "einstein-data.einstein.svc/", "prokube-dk1.adform.zone:22224/" },
            { "einstein-data/", "prokube-dk1.adform.zone:22224/" },
            { "inlb.app.adform.com:20097/", "prokube-dk1.adform.zone:20098/" }
        };

        string jsonFile = File.ReadAllText(jsonFilePath);
        JsonArray? jArray = JsonNode.Parse(jsonFile)?.AsArray();
        
        bool? replacedAnyRows = jArray?.TryReplaceRows(column, replaceMap);
        
        Assert.That(replacedAnyRows, Is.True, "No rows were replaced. Target file not overwritten");
        
        File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jArray));
        
        Console.WriteLine("Some rows were replaced. Target file overwritten");
    }
    
    [Test]
    public void FilterBySortByThenOutputFields()    // WIP filtering doesnt work - sorting works
    {
        string jsonFilePath = "xxx";

        string jsonFile = File.ReadAllText(jsonFilePath);
        JsonArray? jArray = JsonNode.Parse(jsonFile)?.AsArray();
        var temp = jArray.OrderBy(val=> (DateTimeOffset.Parse(val["lastTimestamp"]?.GetValue<string>() ?? DateTimeOffset.MinValue.ToString()))).Where(val => val["metadata"]?["name"].ToString() == "buyside-newton-report-builder-85756645f5-tdjgr").ToArray();
        File.WriteAllText(jsonFilePath+"sorted.json", JsonSerializer.Serialize(temp));
    }
    
    [Test]
    public void MapExcelColumnoJsonArrayOutput() // WIP. something wrong with nuget formatting. doesnt like encoding, yet it says thaT it supports it
    {
        string filePath= "C:\\Users\\l.navasinskas\\OneDrive - Adform\\Documents\\Campaign list.xlsx";

        var excelData = ExcelFileInput(filePath).ToArray();
        Console.WriteLine(excelData);
    }

    private static IEnumerable<string> ExcelFileInput(string filePath)
    {
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:

                // 1. Use the reader methods
                do
                {
                    while (reader.Read())
                    {
                        yield return reader.GetString(0);
                    }
                } while (reader.NextResult());
            }
        }
    }

    [Test]
    public void Random()
    {
        int[] arr= {}; //
        
        Console.WriteLine("Count: "+ arr.Length);
    }

[Test]
    public void IsLeftSupersetOfRight()
    {
        string[] a = {"c:inventory_stats_ssp","c:inventory_stats_cd","c:inventory_stats_adx","c:inventory_stats_ppas","c:inventory_stats_ssp_requests","c:inventory_stats_deal_requests","c:inventory_stats_deal","c:inventory_stats_lap","c:inventory_stats_package_requests","c:inventory_stats_package","c:inventory_stats_creative_requests","c:inventory_stats_creative","c:page_key_value_stats_adx"};
        string[] b = {"c:inventory_stats_ssp","c:web_BidResponseStats","c:web_bidResponsesStats_creative","c:inventory_stats_cd","c:inventory_stats_adx","c:inventory_stats_ppas","c:inventory_stats_ssp_requests","c:inventory_stats_deal_requests","c:inventory_stats_deal","c:inventory_stats_lap","c:inventory_stats_package_requests","c:inventory_stats_package","c:inventory_stats_creative_requests","c:inventory_stats_creative","c:page_key_value_stats_adx","c:geo_technical_stats","c:ssp_auction_performance_stats","c:calculated_billing_record_banners_monthly_snapshot","c:calculated_billing_record_monthly_snapshot","c:calculated_ppas_banners_fees_monthly_snapshot"};
        string[][] aa = new string[][] {a}; // expected
        string[][] ab = new string[][] {a,b}; // actual
        Assert.True(IsLeftSetSupersetOfRight(ab,aa)); // Does actual have the same values as expected
    }
        
        private static bool IsLeftSetSupersetOfRight(string[][]? leftColorSets, string[][]? rightColorSets)
    {
        return leftColorSets is not null && rightColorSets is not null 
                                         && leftColorSets.All(leftColorSet =>
                                             rightColorSets.Any(rightColorSet =>
                                                 rightColorSet.OrderBy(v => v)
                                                     .SequenceEqual(
                                                         leftColorSet.OrderBy(v=>v))));
    }
}