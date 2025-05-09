using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(MainRoute)]
public class IndexController : BaseController
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}
