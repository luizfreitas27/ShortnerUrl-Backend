namespace ShortnerUrl.Api.Dtos.User.Request;

public class UserRegisterRequestDto
{
    public string Email { get; set; }
    public string Username { get; set;  }
    public string Password { get; set;  }
    public string ConfirmPassword { get; set; }
    
}