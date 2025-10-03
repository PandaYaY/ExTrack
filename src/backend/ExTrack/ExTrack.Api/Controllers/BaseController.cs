using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ExTrack.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
public abstract class BaseController : ControllerBase
{
    protected const string MainRoute       = "api/v{version:apiVersion}/";
    protected const string ControllerRoute = "api/v{version:apiVersion}/[controller]";
}
