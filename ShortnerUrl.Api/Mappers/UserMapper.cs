using Mapster;
using ShortnerUrl.Api.Dtos.User.Request;
using ShortnerUrl.Api.Dtos.User.Response;
using ShortnerUrl.Api.Enums;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappers;

public class UserMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRegisterRequestDto, User>()
            .Map(dest => dest.Password, src => BCrypt.Net.BCrypt.HashPassword(src.Password))
            .Map(dest => dest.RoleId, _ => (int) RoleType.User)
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
            .Map(dest => dest.RefreshToken, src => string.Empty)
            .Map(dest => dest.ExpiresAt, src => (DateTime?)null)
            .Map(dest => dest.UpdatedAt, src => (DateTime?)null);

        config.NewConfig<User, UserResponseDto>()
            .Map(dest => dest.RoleName, src => ((RoleType)src.RoleId).ToString());
    }

}