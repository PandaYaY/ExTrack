using ExTrack.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(ControllerRoute)]
public class UsersController(IUsersRepository repository) : BaseController
{
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        var user = await repository.GetUserById(userId);
        if (user == null) return NotFound("User not found");
        return Ok(user);
    }
}
