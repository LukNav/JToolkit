using JToolkit.Authorization;
using JToolkit.Comparer.Models;
using JToolkit.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JToolkit.Controllers;

[ApiController]
[Route("v1/json/[action]")]
public class JsonController : ControllerBase
{
    private readonly IComparisonHandler _handler;

    public JsonController(IComparisonHandler handler) // TODO: move to primary constructor
    {
        _handler = handler;
    }
    
    [ActionName("compare")]
    [HttpPost]
    [Authorize(AuthorizationPolicies.ScopePolicy)]
    public IActionResult Compare([FromBody] ComparisonRequest request)
    {
        var comparisonResult = _handler.Handle(request);
        return Ok(comparisonResult);
    }

}