using Mapster;
using ShortnerUrl.Api.Dtos.Admin;
using ShortnerUrl.Api.Enums;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappers;

public class AdminMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AdminSeedDto, User>()
            .Map(dest => dest.Password, src => BCrypt.Net.BCrypt.HashPassword(src.Password))
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
            .Map(dest => dest.RoleId, _ => (int)RoleType.Admin)
            .Map(deste => deste.UpdatedAt, _ => (DateTime?)null)
            .Map(dest => dest.RefreshToken, _ => string.Empty)
            .Map(dest => dest.ExpiresAt, _ => (DateTime?)null);
    }
}