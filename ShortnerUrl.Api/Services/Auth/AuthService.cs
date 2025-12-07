using ShortnerUrl.Api.Dtos.Auth.Request;
using ShortnerUrl.Api.Dtos.Auth.Response;
using ShortnerUrl.Api.Enums;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Shared;
using ShortnerUrl.Api.Shared.Auth;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Services.Auth;

public class AuthService : IAuthService
{

    private IUnityOfWork _unityOfWork;
    private IJwtTokenProvider _jwtTokenProvider;
    private ILogger<AuthService>  _logger;

    public AuthService(IUnityOfWork unityOfWork, ILogger<AuthService> logger, IJwtTokenProvider jwtTokenProvider)
    {
        _unityOfWork = unityOfWork;
        _logger = logger;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user login {Username}...", dto.Username);
        
        var user = await _unityOfWork.Users.GetByUsernameAsync(dto.Username, cancellationToken);

        if (user == null)
        {
            _logger.LogError("Username or  password is incorrect");
            throw new AppException("Username or password is incorrect", 401);
        }
        
        _logger.LogInformation("Starting password verification...");

        var passwordVerified = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        
        if (!passwordVerified)
        {
            _logger.LogError("Username or password is incorrect");
            throw new AppException("Username or password is incorrect", 401);
        }
        
        _logger.LogInformation("Getting token...");

        var (accessToken, expiresAt) = _jwtTokenProvider.GenerateToken(user);
        var refreshToken = _jwtTokenProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.ExpiresAt = DateTime.UtcNow.AddDays(7);
        
        _unityOfWork.Users.Update(user);
        await _unityOfWork.CommitAsync(cancellationToken);
        
        _logger.LogInformation("The login has been completed successfully...");

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            RefreshToken = refreshToken,
            Username = user.Username,
            RoleId = (RoleType)user.RoleId,
            RoleName = user.Role?.Name ?? user.RoleId.ToString()
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting refresh token...");

        var user = await _unityOfWork.Users.GetByRefreshTokenAsync(dto.RefreshToken, cancellationToken);

        if (user?.RefreshToken == null || user.ExpiresAt <= DateTime.UtcNow)
        {
            _logger.LogInformation("Refresh token is invalid");
            throw new AppException("Refresh token is invalid", 401);
        }

        var (accessToken, expiresAt) = _jwtTokenProvider.GenerateToken(user);
        var newRefreshToken = _jwtTokenProvider.GenerateRefreshToken();
        
        _unityOfWork.Users.Update(user);

        await _unityOfWork.CommitAsync(cancellationToken);
        
        _logger.LogInformation("The Refresh Token has been generated successfully... ");

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            RoleId = (RoleType)user.RoleId,
            RoleName = user.Role?.Name ?? user.RoleId.ToString()
        };
    }
}