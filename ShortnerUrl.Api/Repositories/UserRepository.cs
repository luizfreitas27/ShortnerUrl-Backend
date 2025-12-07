using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Persistence;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShortnerUrlContext _context;

    public UserRepository(ShortnerUrlContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Entry(user).Property(u => u.RefreshToken).IsModified = true;
        _context.Entry(user).Property(u => u.ExpiresAt).IsModified = true;
        _context.Entry(user).Property(u => u.UpdatedAt).IsModified = true;
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}