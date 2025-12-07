using ShortnerUrl.Api.Enums;

namespace ShortnerUrl.Api.Models;

public class User : BaseModel
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    public virtual Role Role { get; set; }
    public virtual IEnumerable<Link>  Links { get; set; } = new List<Link>();
    
    public RoleType RoleType => (RoleType)RoleId;
    
}
