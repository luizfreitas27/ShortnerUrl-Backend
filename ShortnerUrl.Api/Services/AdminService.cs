using Mapster;
using MapsterMapper;
using ShortnerUrl.Api.Dtos.User.Response;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Shared;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Services;

public class AdminService : IAdminService
{
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILogger<AdminService> _logger;

    public AdminService(IMapper mapper, IUnityOfWork unityOfWork, ILogger<AdminService> logger)
    {
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _logger = logger;
    }


    public async Task<List<UserResponseDto>> GetAllUsers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all users...");
        
        var users = await _unityOfWork.Users.GetAllAsync(cancellationToken);

        var list = users?.ToList() ?? [];
        
        if (list.Count == 0)  
        {
            _logger.LogWarning("No users found");
            throw new AppException("No users found...", 404);
        }
        
        _logger.LogInformation("Returning all users...");
        
        return _mapper.From(list).AdaptToType<List<UserResponseDto>>();
    }
    
    public async Task<UserResponseDto> GetUserById(int userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting a user by id...");

        var user = await _unityOfWork.Users.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("No user found with id {userId}...", userId);
            throw new AppException("No user found with id {userId}...", userId);
        }
        
        _logger.LogInformation("Returning user by id {userId}...", userId);

        return _mapper.From(user).AdaptToType<UserResponseDto>();
    }
}