using JToolkit.Authorization;
using JToolkit.Comparer.Models;
using JToolkit.Handlers;
using JToolkit.Http;
using JToolkit.Http.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JToolkit.Controllers;

[ApiController]
[Route("v1/http/[action]")]
public class HttpController(IHttpResponseComparisonHandler handler) : ControllerBase
{
    [ActionName("compareResponses")]
    [HttpPost]
    [Authorize(AuthorizationPolicies.ScopePolicy)]
    public IActionResult ComparePost([FromBody] HttpResponseComparisonRequest request)
    {
        var comparisonResult = handler.Handle(request);
        return Ok(comparisonResult);
    }
}