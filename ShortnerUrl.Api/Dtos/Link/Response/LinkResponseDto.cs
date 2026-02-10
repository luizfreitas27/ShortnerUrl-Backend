namespace ShortnerUrl.Api.Dtos.Link.Response;

public class LinkResponseDto
{
   public Guid Id { get; set; }
   public string Name { get; set; }
   public string OriginalUrl { get; set; }
   public string Shortner { get; set; }
   public DateTime CreatedAt { get; set; }
   public DateTime? UpdatedAt { get; set; }
}