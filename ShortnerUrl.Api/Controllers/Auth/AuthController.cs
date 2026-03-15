using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShortnerUrl.Api.Configurations;
using ShortnerUrl.Api.Dtos.Auth.Request;
using ShortnerUrl.Api.Dtos.Auth.Response;
using ShortnerUrl.Api.Shared.Auth;

namespace ShortnerUrl.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("sign-in")]
    [EnableRateLimiting(RateLimitingConfig.LoginPolicy)]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await _service.LoginAsync(dto, cancellationToken);

        return OkResponse(response);
    }

    [HttpPost]
    [Route("refresh-token")]
    [EnableRateLimiting(RateLimitingConfig.RefreshTokenPolicy)]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto,
        CancellationToken cancellationToken)
    {
        var response = await _service.RefreshTokenAsync(dto, cancellationToken);

        return OkResponse(response);
    }
    
}