using Asp.Versioning;
using ExTrack.Api.Dto;
using ExTrack.Users;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(ControllerRoute)]
[ApiVersion("1.0")]
public class UsersController(IUsersService service) : BaseController
{
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        var user = await service.GetUserById(userId);
        if (user == null) return NotFound("User not found");

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        try
        {
            var user = await service.CreateUser(createUserDto.RoleId, createUserDto.Login, createUserDto.PasswordHash);
            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, user);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
