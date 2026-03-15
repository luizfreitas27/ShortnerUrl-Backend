using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShortnerUrl.Api.Configurations;
using ShortnerUrl.Api.Dtos.Link.Request;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting(RateLimitingConfig.ApiPolicy)]
public class LinkController : BaseController
{
    private readonly ILinkService _service;

    public LinkController(ILinkService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LinkCreateRequestDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var response = await _service.CreateAsync(userId, dto, cancellationToken);

        return CreatedAtResponse(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var response = await _service.GetAllAsync(userId, cancellationToken);
        return OkResponse(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var response = await _service.GetByIdAsync(id, userId, cancellationToken);

        return OkResponse(response);

    }

    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] LinkUpdateRequestDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var response = await _service.UpdateAsync(id, userId, dto, cancellationToken);

        return OkResponse(response);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        
        var userId = GetUserId();

        await _service.DeleteAsync(id, userId, cancellationToken);

        return NoContent();
    }
}