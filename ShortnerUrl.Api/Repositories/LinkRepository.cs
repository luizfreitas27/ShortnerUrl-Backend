using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Persistence;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Repositories;

public class LinkRepository : ILinkRepository
{
    private readonly ShortnerUrlContext _context;

    public LinkRepository(ShortnerUrlContext context)
    {
        _context = context;
    }

    public void Add(Link link)
    {
        _context.Links.Add(link);
    }

    public void Delete(Link link)
    {
        _context.Links.Remove(link);
    }

    public void Update(Link link)
    {
        _context.Links.Update(link);
    }

    public async Task<IEnumerable<Link>> GetAllLinksAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Links
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Where(l => l.UserId == userId )
            .ToListAsync(cancellationToken);
    }

    public async Task<Link?> GetLinkByIdAsync(Guid id, int userId, CancellationToken cancellationToken)
    {
        return await _context.Links
            .Where(l => l.UserId == userId)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Link?> GetLinkByOriginalLinkAsync(int userId, string originalLink, CancellationToken cancellationToken)
    {
        return await _context.Links
            .AsNoTracking()
            .Where(l => l.UserId == userId)
            .FirstOrDefaultAsync(l => l.OriginalUrl ==  originalLink, cancellationToken);
    }

    public async Task<Link?> GetLinkByShortner(string shortner, CancellationToken cancellationToken)
    {
       return await _context.Links
           .AsNoTracking()
           .FirstOrDefaultAsync(l => l.Shortner == shortner, cancellationToken);
    }
}