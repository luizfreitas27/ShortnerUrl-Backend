using Mapster;
using MapsterMapper;
using ShortnerUrl.Api.Dtos.User.Request;
using ShortnerUrl.Api.Dtos.User.Response;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Shared;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Services;

public class UserService : IUserService
{
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILogger<UserService> _logger;
    private IMapper _mapper;

    public UserService(IUnityOfWork unityOfWork, ILogger<UserService> logger, IMapper mapper)
    {
        _unityOfWork = unityOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async  Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user registration...");
        
        _logger.LogInformation("Verifying Email...");

        var email = await _unityOfWork.Users.GetByEmailAsync(dto.Email, cancellationToken);
        if (email != null)
        {
            _logger.LogInformation("Email already registered...");
            throw new AppException("Email already registered", 409);
        }
        
        _logger.LogInformation("Verifying Username...");
        
        var username = await _unityOfWork.Users.GetByUsernameAsync(dto.Username, cancellationToken);
        if (username != null)
        {
            _logger.LogInformation("Username already registered...");
            throw new AppException("Username already registered", 409);
        }

        var newUser = _mapper.From(dto).AdaptToType<User>();
        
        _unityOfWork.Users.Add(newUser);
        
        await _unityOfWork.CommitAsync(cancellationToken);
        
        _logger.LogInformation("User created successfully.");

        return _mapper.From(newUser).AdaptToType<UserResponseDto>();

    }

    public Task<UserResponseDto> UpdateAsync(UserUpdateRequestDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}