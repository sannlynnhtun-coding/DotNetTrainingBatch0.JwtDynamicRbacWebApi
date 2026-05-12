namespace DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Auth;

public class AuthRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthResponse
{
    public string AccessToken { get; set; } = "";
    public string Role { get; set; } = "";
    public List<string> Permissions { get; set; } = new();
}
