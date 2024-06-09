using JToolkit.Comparer.Comparer;
using JToolkit.Comparer.Mappers;
using JToolkit.Comparer.Models;
using JToolkit.Handlers;
using Newtonsoft.Json;

namespace JToolkit.Integration.Tests;

public class ComparisonTests
{
    private ComparisonHandler _comparisonHandler { get; set; } // TODO: Go through responses, make sure they all make sense
    
    [OneTimeSetUp]
    public void Setup()
    {
        IComparisonMapper comparisonMapper = new ComparisonMapper();
        IJsonComparer jsonComparer = new JsonComparer();
        _comparisonHandler = new ComparisonHandler(comparisonMapper, jsonComparer);
    }

    [TestCase("""
              {
                "metric":"Tes.t"
              }
              """, TestName="DifferencesReturned_WhenValuesAreNotEqual")]
    [TestCase("""
              {
                "metric": {"value":"Tes.t1"}
              }
              """, TestName="DifferencesReturned_WhenValuesAreDifferentTypes")]
    [TestCase("""
              {
                "metric": null
              }
              """, TestName="DifferencesReturned_WhenActualValueIsNull")]
    public void ValuesReturnsDifferences(string actualValue)
    {
        string expected = """
                          {
                            "metric":"Tes.t1"
                          }
                          """;
        
        string actual = actualValue;
        
        var request = new ComparisonRequest
        {
            Actual = actual, Expected = expected
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.False);
        Assert.That(actualResponse.Differences, Is.Not.Empty);

        Console.WriteLine($"Response:{Environment.NewLine}{JsonConvert.SerializeObject(actualResponse)}"); 
    }

    [Test]
    public void NoDifferencesReturned_WhenValuesAreBothNull()
    {
        string jsonVal = """
                          {
                            "metric": null
                          }
                          """;
        
        var request = new ComparisonRequest
        {
            Actual = jsonVal, Expected = jsonVal
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.True);
        Assert.That(actualResponse.Differences, Is.Null);

        Console.WriteLine($"Response:{Environment.NewLine}{JsonConvert.SerializeObject(actualResponse)}"); 
    }
    
    [TestCase("""
              {
                "metrics": ["TestVal1"]
              }
              """, TestName="DifferencesReturned_WhenValuesAreMissing")]
    [TestCase("""
              {
                "metrics": ["TestVal1", "TestVal2", "TestVal3"]
              }
              """, TestName="DifferencesReturned_WhenNewValuesFound")]
    [TestCase("""
              {
                "metrics": ["TestVal1", "TestVal3"]
              }
              """, TestName="DifferencesReturned_WhenValuesAreDifferent")]
    [TestCase("""
              {
                "metrics": ["TestVal1", null]
              }
              """, TestName="DifferencesReturned_WhenSomeValuesAreNull")]
    [TestCase("""
              {
                "metrics": ["TestVal1", "TestVal2", null]
              }
              """, TestName="DifferencesReturned_WhenArraysContainAllValuesAndNull")]
    [TestCase("""
              {
                "metrics": null
              }
              """, TestName="DifferencesReturned_WhenArraysIsNull")]
    [TestCase("""
              {
                "metrics": [null,null]
              }
              """, TestName="DifferencesReturned_WhenAllValuesAreNull")]
    public void ArrayValuesReturnsDifferences(string actualValue)
    {
        string expected = """
                          {
                            "metrics": ["TestVal1", "TestVal2"]
                          }
                          """;
        
        string actual = actualValue;
        
        var request = new ComparisonRequest
        {
            Actual = actual, Expected = expected
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.False);
        Assert.That(actualResponse.Differences, Is.Not.Null);
        Assert.That(actualResponse.Differences, Is.Not.Empty);

        Console.WriteLine($"Response:{Environment.NewLine}{JsonConvert.SerializeObject(actualResponse)}"); 
    }
    
    [TestCase("""
              {
                "metrics": ["TestVal1", "TestVal2"]
              }
              """, TestName="NoDifferencesReturned_WhenAllValuesAreSame")]
    [TestCase("""
        {
          "metrics": ["TestVal2", "TestVal1"]
        }
        """, TestName="NoDifferencesReturned_WhenAllValuesAreSameButDifferentOrder")]
    public void ArrayValuesReturnsNoDifferences(string actualValue)
    {
        string expected = """
                          {
                            "metrics": ["TestVal1", "TestVal2"]
                          }
                          """;
        
        string actual = actualValue;
        
        var request = new ComparisonRequest
        {
            Actual = actual, Expected = expected
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.True);
        Assert.That(actualResponse.Differences, Is.Null);
    }
      
    [TestCase("""
              {
                "Employees": [ { "firstName":"Johnny" }]
              }
              """, TestName="DifferencesReturned_WhenObjectValuesAreDifferent")]
    [TestCase("""
              {
                "Employees": [ { "firstName":"John" }, { "firstName":"Bob" }]
              }
              """, TestName="DifferencesReturned_WhenActualHasMoreValues")]
    public void ObjectArraysReturnsDifferences(string actualValue)
    {
        string expected = """
                          {
                            "Employees": [ { "firstName":"John" }]
                          }
                          """;
        
        string actual = actualValue;
        
        var request = new ComparisonRequest
        {
            Actual = actual, Expected = expected
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.False);
        Assert.That(actualResponse.Differences, Is.Not.Null);
        Assert.That(actualResponse.Differences, Is.Not.Empty);

        Console.WriteLine($"Response:{Environment.NewLine}{JsonConvert.SerializeObject(actualResponse)}"); 
    }
    
      
    [TestCase("""
              {
                "Employees": [ { "firstName":"John" }]
              }
              """, TestName="NoDifferencesReturned_WhenObjectValuesAreEquivalent")]
    public void ObjectArraysReturnsNoDifferences(string actualValue)
    {
        string expected = """
                          {
                            "Employees": [ { "firstName":"John" }]
                          }
                          """;
        
        string actual = actualValue;
        
        var request = new ComparisonRequest
        {
            Actual = actual, Expected = expected
        };

        var actualResponse = _comparisonHandler.Handle(request);
        
        Assert.That(actualResponse.AreEquivalent, Is.True);
        Assert.That(actualResponse.Differences, Is.Null.Or.Empty);
    }
}