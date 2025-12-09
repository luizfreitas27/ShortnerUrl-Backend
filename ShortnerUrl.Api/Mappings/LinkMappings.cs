using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappings;

public class LinkMappings : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("tb_links");
        
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedNever();
        
        builder.Property(l => l.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(l => l.OriginalUrl)
            .HasColumnName("original_url")
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(l => l.Shortner)
            .HasColumnName("shortner")
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.Property(r => r.UpdatedAt)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("updated_at");
        
        builder.HasOne(l => l.User)
            .WithMany(u => u.Links)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}