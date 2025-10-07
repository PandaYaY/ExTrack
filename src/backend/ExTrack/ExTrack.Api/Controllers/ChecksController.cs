using ExTrack.Api.Dto.Checks;
using ExTrack.Checks;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(ControllerRoute)]
public class ChecksController(ILogger<ChecksController> logger, IChecksService checksService) : BaseController
{
    [HttpGet("{checkId:int}")]
    public async Task<IActionResult> GetCheck(int checkId)
    {
        var check = await checksService.GetCheckById(checkId);
        return Ok(check);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserChecks([FromQuery(Name = "user_id")]  int userId,
                                                   [FromQuery(Name = "page")]     int page    = 1,
                                                   [FromQuery(Name = "per_page")] int perPage = 10)
    {
        var checks = await checksService.GetUserChecks(userId, page, perPage);
        return Ok(checks);
    }

    [HttpPost]
    public async Task<IActionResult> AddCheckInfo(GetCheckInfoDto checkInfoDto)
    {
        var checkId = await checksService.GetCheckInfoAsync(checkInfoDto);
        return CreatedAtAction(nameof(GetCheck), checkId);
    }
}
