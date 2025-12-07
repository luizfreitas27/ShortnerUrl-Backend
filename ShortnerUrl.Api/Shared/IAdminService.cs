using ShortnerUrl.Api.Dtos.User.Response;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Shared;

public interface IAdminService
{
    Task<List<UserResponseDto>> GetAllUsers(CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserById(int userId, CancellationToken cancellationToken);
}