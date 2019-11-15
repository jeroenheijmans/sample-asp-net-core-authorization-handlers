using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foo.Api.Controllers
{
    [Authorize(Policy = nameof(Policies.MyPolicy))]
    [ApiController]
    [Route("[controller]")]
    public class BarController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet] public string Get() => "Index result is public (AllowAnonymous)";

        // Endpoints reusing Authorize attribute on controller
        [HttpGet("a")] public string GetA() => "Result A";
        [HttpGet("b")] public string GetB() => "Result B";
        [HttpGet("etc")] public string GetEtc() => "Et cetera; many other route/verb combinations";

        // Endpoint that is "semi-public", so *less* strict than the default:
        [HttpGet("semi-protected")] public string GetSemiProtected() => "Single semi-protected endpoint open to specific users";
    }
}
