using ExTrack.Api.Dto.Users;
using ExTrack.Users;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[Route(ControllerRoute)]
public class UsersController(ILogger<UsersController> logger, IUsersService service) : BaseController
{
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        try
        {
            var user = await service.GetUserById(userId);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error getting user with id {UserId}", userId);
            return BadRequest(exception.Message);
        }
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
            logger.LogWarning(exception, "Error create user with params: [{Login}, {Role}]", createUserDto.Login,
                              createUserDto.RoleId);
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error create user with params: [{Login}, {Role}]", createUserDto.Login,
                            createUserDto.RoleId);
            return BadRequest(exception.Message);
        }
    }
}
