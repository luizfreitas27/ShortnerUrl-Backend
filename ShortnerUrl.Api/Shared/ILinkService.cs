using ShortnerUrl.Api.Dtos.Link.Request;
using ShortnerUrl.Api.Dtos.Link.Response;

namespace ShortnerUrl.Api.Shared;

public interface ILinkService
{
    Task<LinkResponseDto> CreateAsync(int userId, LinkCreateRequestDto dto, CancellationToken cancellationToken);
    Task<LinkResponseDto> GetByIdAsync(Guid id, int userId, CancellationToken cancellationToken);
    Task<List<LinkResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken);
    Task<LinkResponseDto> UpdateAsync(Guid id, int userId, LinkUpdateRequestDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, int userId, CancellationToken cancellationToken);
    



}