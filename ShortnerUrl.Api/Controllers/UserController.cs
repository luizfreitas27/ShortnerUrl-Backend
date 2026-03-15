using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShortnerUrl.Api.Configurations;
using ShortnerUrl.Api.Dtos.User.Request;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("sign-up")]
    [EnableRateLimiting(RateLimitingConfig.LoginPolicy)]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await _service.RegisterAsync(dto, cancellationToken);

        return CreatedResponse($"/api/User/{response.Id}", response);
    }
}