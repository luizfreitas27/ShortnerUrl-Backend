namespace ShortnerUrl.Api.Models;

public class Link 
{
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string OriginalUrl { get; set; }
    public required string Shortner { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public required int UserId { get; set; }
    
    public virtual User User { get; set; }
    
}