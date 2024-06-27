using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Comparer.Models;
using JToolkit.Controllers;
using JToolkit.Handlers;

namespace JToolkit.Integration.Tests;

public class HappyFlowTests
{
    private ComparisonHandler _comparisonHandler { get; set; }
    
    [OneTimeSetUp]
    public void Setup()
    {
        IComparisonMapper comparisonMapper = new ComparisonMapper();
        IJsonComparer jsonComparer = new JsonComparer();
        _comparisonHandler = new ComparisonHandler(comparisonMapper, jsonComparer);
    }

    [Test]
    public void HappyFlow_EqualValues()
    {
        string jsonVal = """
                         {
                           "testVal":"Tes.t1"
                         }
                         """;
        
        var request = new ComparisonRequest
        {
            Actual = jsonVal, Expected = jsonVal
        };

        // var expectedResponse = new ComparisonResult(true, null);
        var actualResponse = _comparisonHandler.Handle(request);
        
        // Assert.That(actualResponse.AreEquivalent, Is.EqualTo(expectedResponse.AreEquivalent));
        // Assert.That(actualResponse.Differences, Is.EqualTo(expectedResponse.Differences));
    }
    
    [Test]
    public void HappyFlow_EqualArrays()
    {
        string jsonVal = """
                         {
                           "testArr":
                            [
                                "valueA",
                                "valueB"
                            ]
                         }
                         """;
        
        var request = new ComparisonRequest
        {
            Actual = jsonVal, Expected = jsonVal
        };

        // var expectedResponse = new ComparisonResult(true, null);
        // var actualResponse = _comparisonHandler.Handle(request);
        //
        // Assert.That(actualResponse.AreEquivalent, Is.EqualTo(expectedResponse.AreEquivalent));
        // Assert.That(actualResponse.Differences, Is.EqualTo(expectedResponse.Differences));
    }
}