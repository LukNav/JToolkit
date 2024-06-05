using NUnit.Framework;
using HttpClient = JToolkit.Playground.Random.HttpTools.HttpClient;

namespace JToolkit.Playground.Random;

[Explicit]
public class HttpPlayground
{
    [Test]
    public void CreateDeals()
    {
        int amountOfDealsCreated = 500;
        
        string token="awdawdawd";
        string url = "http://inlb.pre1.root.adform.com:10092";
        string dealsEndpoint = "/DirectIntegrations/Deals";
        // string dealServiceEndpoint = "/DealService/ChangeDealState/2";
        
        // HttpClient client = new HttpClient().WithBearerToken(token);
        HttpClient client = new HttpClient().WithAuthTicket(token);
        
        for (int i = 0; i < amountOfDealsCreated; i++)
        {
            Console.WriteLine($"\nCreating deal {i + 1}");
            var request = CreateDealsRequst();
            var response = client.PostAsync(url + dealsEndpoint, request).Result;
            // var activationResponse = client.GetAsync(url + dealServiceEndpoint).Result;
            
            // Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            // Console.WriteLine(activationResponse);
        }
    }

    private string CreateDealsRequst()
    {
        var randomPostFix = Guid.NewGuid().ToString();
        string request = $$"""
                           {
                               "EstimatedImpressionsType": 1,
                               "DealState": {
                                   "Id": 1
                               },
                               "UserGroupIds": [],
                               "AdvertiserAccessRulesBypassed": false,
                               "PricingType": 1,
                               "DealType": 1,
                               "ClientDivisionIds": [],
                               "Placements": [
                                   {
                                       "PlacementId": 202282,
                                       "CreativeSettingIds": [
                                           39649,
                                           39650
                                       ]
                                   }
                               ],
                               "CreativeIds": [],
                               "PriorityType": 2,
                               "Fee": null,
                               "InventorySource": {
                                   "AgencyId": 541,
                                   "DefaultMediaId": 133531,
                                   "SupportsPreferredDeals": true,
                                   "IsApprovalRequired": true,
                                   "AdxDomainId": 0,
                                   "MaxPlacementsCount": 100,
                                   "CountryId": 48,
                                   "Id": 450,
                                   "Name": "Vladimir",
                                   "Currency": "USD",
                                   "PartyType": 7,
                                   "TimeZone": {
                                       "Id": "Kaliningrad Standard Time",
                                       "DisplayName": "(UTC+02:00) Kaliningrad",
                                       "StandardName": "Russia TZ 1 Standard Time",
                                       "DaylightName": "Russia TZ 1 Daylight Time",
                                       "BaseUtcOffset": "02:00:00",
                                       "AdjustmentRules": [
                                           {
                                               "DateStart": "0001-01-01T00:00:00",
                                               "DateEnd": "2010-12-31T00:00:00",
                                               "DaylightDelta": "01:00:00",
                                               "DaylightTransitionStart": {
                                                   "TimeOfDay": "0001-01-01T02:00:00",
                                                   "Month": 3,
                                                   "Week": 5,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": false
                                               },
                                               "DaylightTransitionEnd": {
                                                   "TimeOfDay": "0001-01-01T03:00:00",
                                                   "Month": 10,
                                                   "Week": 5,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": false
                                               },
                                               "BaseUtcOffsetDelta": "00:00:00"
                                           },
                                           {
                                               "DateStart": "2011-01-01T00:00:00",
                                               "DateEnd": "2011-12-31T00:00:00",
                                               "DaylightDelta": "01:00:00",
                                               "DaylightTransitionStart": {
                                                   "TimeOfDay": "0001-01-01T02:00:00",
                                                   "Month": 3,
                                                   "Week": 5,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": false
                                               },
                                               "DaylightTransitionEnd": {
                                                   "TimeOfDay": "0001-01-01T00:00:00",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 6,
                                                   "IsFixedDateRule": false
                                               },
                                               "BaseUtcOffsetDelta": "00:00:00"
                                           },
                                           {
                                               "DateStart": "2012-01-01T00:00:00",
                                               "DateEnd": "2012-12-31T00:00:00",
                                               "DaylightDelta": "00:00:00",
                                               "DaylightTransitionStart": {
                                                   "TimeOfDay": "0001-01-01T00:00:00",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": true
                                               },
                                               "DaylightTransitionEnd": {
                                                   "TimeOfDay": "0001-01-01T00:00:00.001",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": true
                                               },
                                               "BaseUtcOffsetDelta": "01:00:00"
                                           },
                                           {
                                               "DateStart": "2013-01-01T00:00:00",
                                               "DateEnd": "2013-12-31T00:00:00",
                                               "DaylightDelta": "00:00:00",
                                               "DaylightTransitionStart": {
                                                   "TimeOfDay": "0001-01-01T00:00:00",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": true
                                               },
                                               "DaylightTransitionEnd": {
                                                   "TimeOfDay": "0001-01-01T00:00:00.001",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": true
                                               },
                                               "BaseUtcOffsetDelta": "01:00:00"
                                           },
                                           {
                                               "DateStart": "2014-01-01T00:00:00",
                                               "DateEnd": "2014-12-31T00:00:00",
                                               "DaylightDelta": "01:00:00",
                                               "DaylightTransitionStart": {
                                                   "TimeOfDay": "0001-01-01T00:00:00",
                                                   "Month": 1,
                                                   "Week": 1,
                                                   "Day": 1,
                                                   "DayOfWeek": 3,
                                                   "IsFixedDateRule": false
                                               },
                                               "DaylightTransitionEnd": {
                                                   "TimeOfDay": "0001-01-01T02:00:00",
                                                   "Month": 10,
                                                   "Week": 5,
                                                   "Day": 1,
                                                   "DayOfWeek": 0,
                                                   "IsFixedDateRule": false
                                               },
                                               "BaseUtcOffsetDelta": "00:00:00"
                                           }
                                       ],
                                       "SupportsDaylightSavingTime": true
                                   },
                                   "PublisherAdServerEnabled": true
                               },
                               "selectedBuyers": [],
                               "DemandPartnerIds": [],
                               "AllDemandPartnersSelected": true,
                               "Name": "BDP-12079_{{randomPostFix}}",
                               "Price": "2.00",
                               "ValidFrom": "2024-06-03",
                               "ValidTo": "2024-06-16",
                               "EstimatedImpressions": "24334",
                               "BuyerIds": [],
                               "DemandPartners": [],
                               "AdformDmpTargetingRules": []
                           }
                           """;
        return request;
    }
}