using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recepify.API.Models.Auth;
using Recepify.DLL.Entities;

namespace Recepify.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private static readonly Dictionary<string, string> RefreshTokens = new();
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _config = config;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
            
        if (!result.Succeeded)
            return BadRequest(result.Errors);
            
        return Ok("User registered successfully");
    }
        
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, request.Password)))
            return Unauthorized("Invalid credentials");
            
        var token = GenerateJwtToken(user.UserName);
        var refreshToken = GenerateRefreshToken();
        RefreshTokens[user.UserName] = refreshToken;
            
        return Ok(new { Token = token, RefreshToken = refreshToken });
    }
        
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequest request)
    {
        if (!RefreshTokens.TryGetValue(request.Username, out var storedToken) || storedToken != request.RefreshToken)
            return Unauthorized("Invalid refresh token");
            
        var token = GenerateJwtToken(request.Username);
        var newRefreshToken = GenerateRefreshToken();
        RefreshTokens[request.Username] = newRefreshToken;
            
        return Ok(new { Token = token, RefreshToken = newRefreshToken });
    }
    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}