using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Shared.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetRoleById (int id, CancellationToken cancellationToken); 
}