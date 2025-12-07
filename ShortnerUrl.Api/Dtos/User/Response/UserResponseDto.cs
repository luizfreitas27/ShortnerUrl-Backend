using ShortnerUrl.Api.Enums;

namespace ShortnerUrl.Api.Dtos.User.Response;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string RoleName { get; set; }
    public DateTime CreatedAt { get; set; }
}