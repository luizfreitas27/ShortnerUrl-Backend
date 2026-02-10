using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Controllers;

public class RedirectController : BaseController
{
    private readonly ILinkService  _service;

    public RedirectController(ILinkService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Redirect(string id, CancellationToken cancellationToken)
    {

        var response = await _service.RedirectAsync(id, cancellationToken);
        
        return Redirect(response);
    }
    
}