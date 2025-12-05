using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Persistence;

public class ShortnerUrlContext : DbContext
{
    public ShortnerUrlContext(DbContextOptions<ShortnerUrlContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShortnerUrlContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
}