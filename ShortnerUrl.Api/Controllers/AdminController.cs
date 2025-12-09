using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortnerUrl.Api.Constants;
using ShortnerUrl.Api.Dtos.User.Response;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController :  BaseController
{
    private readonly IAdminService _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("users/")]
    [Authorize(Policy = AuthConstants.Policies.RequireAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllUsers(cancellationToken);
        return Ok(users);
    }
    
    [HttpGet]
    [Route("users/{id}")]
    [Authorize(Policy = AuthConstants.Policies.RequireAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var users = await _service.GetUserById(id, cancellationToken);
        return Ok(users);
    }
}