using ShortnerUrl.Api.Dtos.User.Request;
using ShortnerUrl.Api.Dtos.User.Response;

namespace ShortnerUrl.Api.Shared;

public interface IUserService
{

    Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto dto, CancellationToken cancellationToken);
    Task<UserResponseDto> UpdateAsync(UserUpdateRequestDto dto, CancellationToken cancellationToken);

}