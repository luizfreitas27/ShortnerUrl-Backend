using ShortnerUrl.Api.Dtos.Auth.Request;
using ShortnerUrl.Api.Dtos.Auth.Response;

namespace ShortnerUrl.Api.Shared.Auth;

public interface IAuthService
{
   Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken); 
   
   Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken cancellationToken);
}