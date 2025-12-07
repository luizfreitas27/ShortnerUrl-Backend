using Microsoft.AspNetCore.Mvc;
using ShortnerUrl.Api.Dtos.User.Request;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto dto)
    {
        return Ok();
    }

}