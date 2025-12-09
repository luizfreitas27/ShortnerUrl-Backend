using Mapster;
using ShortnerUrl.Api.Dtos.Link.Request;
using ShortnerUrl.Api.Dtos.Link.Response;
using ShortnerUrl.Api.Models;

namespace ShortnerUrl.Api.Mappers;

public class LinkMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LinkCreateRequestDto, Link>()
            .Ignore(dest => dest.User)
            .Ignore(dest => dest.UserId)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
            .Map(dest => dest.UpdatedAt, _ => (DateTime?)null);

        config.NewConfig<LinkUpdateRequestDto, Link>()
            .IgnoreNullValues(true)
            .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow)
            
            ;
        
        
        config.NewConfig<Link, LinkResponseDto>();
    }
}