using Asp.Versioning;
using ExTrack.Api.Dto.Checks;
using ExTrack.Checks;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ChecksController(ILogger<ChecksController> logger, IChecksService checksService) : ControllerBase
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
    public async Task<IActionResult> AddCheck(GetCheckInfoDto checkInfoDto)
    {
        try
        {
            var checkInfo = await checksService.GetCheckInfoAsync(checkInfoDto);
            return CreatedAtAction(nameof(GetCheck), new { checkId = checkInfo.CheckId }, checkInfo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message.Replace($" (Parameter '{ex.ParamName}')", ""));
        }
    }
}
