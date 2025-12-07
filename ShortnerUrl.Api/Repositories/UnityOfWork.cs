using ShortnerUrl.Api.Persistence;
using ShortnerUrl.Api.Shared;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Repositories;

public class UnityOfWork : IUnityOfWork, IAsyncDisposable
{
    private ShortnerUrlContext  _context;
    
    private IUserRepository? _userRepository;
    private IRoleRepository? _roleRepository;

    public UnityOfWork(ShortnerUrlContext context)
    {
        _context = context;
    }
    
    public IUserRepository Users => 
        _userRepository ??= new UserRepository(_context);
    
    public IRoleRepository Roles =>
        _roleRepository ??= new RoleRepository(_context);
    

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}