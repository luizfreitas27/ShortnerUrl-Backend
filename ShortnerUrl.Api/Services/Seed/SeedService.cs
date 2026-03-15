using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShortnerUrl.Api.Dtos.Admin;
using ShortnerUrl.Api.Enums;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Persistence;
using ShortnerUrl.Api.Settings;

namespace ShortnerUrl.Api.Services.Seed;

public class SeedService
{
    private readonly ILogger<SeedService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ShortnerUrlContext  _context;
    private readonly IMapper _mapper;
    private readonly SeedSettings _seedSettings;

    public SeedService(
        ILogger<SeedService> logger, 
        IConfiguration configuration,
        ShortnerUrlContext context,
        IMapper mapper,
        IOptions<SeedSettings> seedSettings)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
        _seedSettings = seedSettings.Value;
    }

    public async Task SeedAdminAsync()
    {
        var adminExist = await _context.Users
            .AnyAsync(u => u.RoleId == (int)RoleType.Admin);

        if (adminExist)
        {
            _logger.LogWarning("Admin user already exists");
            return;
        }
        
        var email = _seedSettings.Email;
        var password = _seedSettings.Password;

        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("AdminSeed:Email is required.");
        }

        if (string.IsNullOrEmpty(password))
        {
            _logger.LogWarning("AdminSeed:Password is required.");
            return;
        }

        var admin = new AdminSeedDto
        {
            Email = email,
            Username = "admin",
            Password = password
        };
        
       
        var userAdmin = _mapper.From(admin).AdaptToType<User>();
        
        _context.Users.Add(userAdmin);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Admin user created successfully.");
    }
}