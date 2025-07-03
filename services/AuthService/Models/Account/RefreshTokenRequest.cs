namespace AuthService.Models.Account;

public class RefreshTokenRequest
{
    public string Username { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}