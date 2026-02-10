namespace ShortnerUrl.Api.Shared.Repositories;

public interface IUnityOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ILinkRepository Links { get; }
    
    Task<int> CommitAsync(CancellationToken cancellationToken);
}