using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShortnerUrl.Api.Dtos.Auth.Request;
using ShortnerUrl.Api.Dtos.Auth.Response;
using ShortnerUrl.Api.Shared.Auth;

namespace ShortnerUrl.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await _service.LoginAsync(dto, cancellationToken);
        
        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto,
        CancellationToken cancellationToken)
    {
        var response = await _service.RefreshTokenAsync(dto, cancellationToken);

        return Ok(response);
    }
    
}