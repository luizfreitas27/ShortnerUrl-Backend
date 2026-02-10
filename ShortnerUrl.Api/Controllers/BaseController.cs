using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase 
{
    protected int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}