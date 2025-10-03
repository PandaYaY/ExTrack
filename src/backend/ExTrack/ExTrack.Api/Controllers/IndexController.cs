using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(MainRoute)]
[ApiVersion("1.0")]
public class IndexController : BaseController
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}
