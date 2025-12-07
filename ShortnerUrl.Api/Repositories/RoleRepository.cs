using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Persistence;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ShortnerUrlContext _context;

    public RoleRepository(ShortnerUrlContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleById(int id, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}