using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappings;

public class UserMappings : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tb_users");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(255);;

        builder.Property(u => u.RoleId)
            .HasColumnName("role_id")
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("TIMESTAMPTZ");
        
        builder.Property(u => u.RefreshToken)
            .HasColumnName("refresh_token")
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(u => u.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("TIMESTAMPTZ");
        
        
        builder.HasMany(u => u.Links)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u =>  u.Role)
            .WithMany(u => u.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(u => u.Username)
            .IsUnique();
    }
}