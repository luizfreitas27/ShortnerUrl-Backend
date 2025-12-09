using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Shared.Repositories;

public interface ILinkRepository
{
    void Add (Link link);
    void Delete (Link link);
    void Update (Link link);
    Task<IEnumerable<Link>> GetAllLinksAsync(int userId, CancellationToken cancellationToken);
    Task<Link?> GetLinkByIdAsync(Guid id, int userId, CancellationToken cancellationToken);
    Task<Link?> GetLinkByOriginalLinkAsync(int userId, string originalLink, CancellationToken cancellationToken);
    Task<Link?> GetLinkByShortner(string shortner, CancellationToken cancellationToken);
}