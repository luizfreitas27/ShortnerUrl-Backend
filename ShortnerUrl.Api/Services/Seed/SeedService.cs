using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Dtos.Admin;
using ShortnerUrl.Api.Enums;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Persistence;

namespace ShortnerUrl.Api.Services.Seed;

public class SeedService
{
    private readonly ILogger<SeedService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ShortnerUrlContext  _context;
    private readonly IMapper _mapper;

    public SeedService(
        ILogger<SeedService> logger, 
        IConfiguration configuration,
        ShortnerUrlContext context,
        IMapper mapper)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
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
        
        var email = _configuration["AdminSeed:Email"];
        var password = _configuration["AdminSeed:Password"];

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
            Email = "admin@admin.com",
            Username = "admin",
            Password = password
        };
        
       
        var userAdmin = _mapper.From(admin).AdaptToType<User>();
        
        _context.Users.Add(userAdmin);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Admin user created successfully.");
    }
}