using NUnit.Framework;
using HttpClient = JToolkit.Playground.Random.HttpTools.HttpClient;

namespace JToolkit.Playground.Random;

[Explicit]
public class HttpPlayground
{
    [Test]
    public void CreateDeals()
    {
        int amountOfDealsCreated = 5;
        
        string bearerToken="awdawdawd";
        string url = "https://seller-deals.app.d1.adform.zone";
        string endpoint = "/v1/seller/deals";
        
        HttpClient client = new(bearerToken);

        for (int i = 0; i < amountOfDealsCreated; i++)
        {
            Console.WriteLine($"\nCreating deal {i + 1}");
            var request = CreateDealsRequst();
            var response = client.PostAsync(url + endpoint, request).Result;
            
            string responseMessage = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseMessage);
        }
    }

    private string CreateDealsRequst()
    {
        var randomPostFix = Guid.NewGuid().ToString();
        string request = $$"""
                           {
                           "name":"BDP-12079_{{randomPostFix}}",
                           "priority":{"dealPriorityType":"pricePriority"},
                           "price":{"type":"fixed","value":"1.00"},
                           "validPeriod":{"from":"2024-05-30","to":"2024-06-09"},
                           "estimatedImpressions":null,
                           "termsAndConditions":null,
                           "placements":[{"id":165890,"creativeSettings":[29179,29182]},{"id":165891,"creativeSettings":[29181]}],
                           "buyers":{"allDemandPartners":true,"agencyIds":null,"allAgencies":true},
                           "targetingRules":{"dmp":null,"keyValuePairs":null,"keywords":null},
                           "advertiserAccessRulesBypassed":false,"fee":null}
                           """;
        return request;
    }
}