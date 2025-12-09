namespace ShortnerUrl.Api.Dtos.Link.Request;

public class LinkCreateRequestDto
{
    public required string Name { get; set; }
    public required string OriginalUrl { get; set; }
}