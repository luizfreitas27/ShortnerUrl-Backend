using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Shared.Repositories;

public interface IUserRepository
{
    void Add(User user);
    void Update(User user);
    void Delete(User user);
    Task<IEnumerable<User>>  GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

}