using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[ApiController]
[Route("api/v1/")]
public abstract class BaseController : ControllerBase
{
    protected const string MainRoute = "api/v1/";
    protected const string ControllerRoute = "api/v1/[controller]";
}
