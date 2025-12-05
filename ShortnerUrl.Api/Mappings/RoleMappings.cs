using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappings;

public class RoleMappings : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("tb_roles");
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(r => r.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.CreatedAt)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("created_at")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("updated_at");
            

        builder.HasIndex(r => r.Name)
            .IsUnique();
    }
}