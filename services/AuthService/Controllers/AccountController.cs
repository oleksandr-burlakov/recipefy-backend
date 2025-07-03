using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Entities;
using AuthService.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private static readonly Dictionary<string, string> RefreshTokens = new();
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IConfiguration config, UserManager<User> userManager,
            SignInManager<User> signInManager)
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
            
            // Add event to message bus that user was registered and json object of user
            
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, request.Password)))
                return BadRequest("Invalid credentials");

            var token = GenerateJwtToken(user.UserName, user.Id);
            var refreshToken = GenerateRefreshToken();
            RefreshTokens[user.UserName] = refreshToken;

            return Ok(new { Token = token, RefreshToken = refreshToken, UserId = user.Id });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!RefreshTokens.TryGetValue(request.Username, out var storedToken) ||
                storedToken != request.RefreshToken)
                return Unauthorized("Invalid refresh token");

            var token = GenerateJwtToken(request.Username, request.UserId);
            var newRefreshToken = GenerateRefreshToken();
            RefreshTokens[request.Username] = newRefreshToken;

            return Ok(new { Token = token, RefreshToken = newRefreshToken });
        }

        private string GenerateJwtToken(string username, Guid id)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.NameIdentifier, id.ToString()) };
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
}