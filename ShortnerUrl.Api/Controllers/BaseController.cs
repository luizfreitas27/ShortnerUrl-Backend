using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShortnerUrl.Api.Common;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    protected OkObjectResult OkResponse<T>(T data) => Ok(ApiResponse<T>.Ok(data));

    protected CreatedResult CreatedResponse<T>(string location, T data) =>
        Created(location, ApiResponse<T>.Ok(data));

    protected CreatedAtActionResult CreatedAtResponse<T>(string actionName, object routeValues, T data) =>
        CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data));
}
