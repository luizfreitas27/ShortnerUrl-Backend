namespace ShortnerUrl.Api.Dtos.Auth.Response;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Username { get; set; }
    public string RoleName { get; set; }
}