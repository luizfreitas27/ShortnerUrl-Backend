using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Shared.Auth;

public interface IJwtTokenProvider
{
    (string token, DateTime ExpiresAt) GenerateToken(User user);
    string GenerateRefreshToken();
    
    
}