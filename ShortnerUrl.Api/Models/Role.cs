namespace ShortnerUrl.Api.Models;

public class Role : BaseModel
{
    public required string Name { get; set; } 
    public required string Description { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    
    public virtual IEnumerable<User> Users { get; set; } = new List<User>();
}