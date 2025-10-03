using ExTrack.Api.Dto.Checks;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(ControllerRoute)]
public class ChecksController : BaseController
{
    [HttpPost]
    public IActionResult AddCheckInfo(CheckInfoDto checkInfoDto)
    {
        return Ok();
    }
}
