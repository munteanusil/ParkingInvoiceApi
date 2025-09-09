using AuthService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly UserManager<AppUser> _users;
        private readonly IConfiguration _cfg;

        public TokenController(UserManager<AppUser> users,IConfiguration cfg)
        {
            _users = users;
            _cfg = cfg;
        }

        public record LoginDto(string Email, string Password);
        public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresUtc);
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(LoginDto dto)
        {
            var u = await _users.FindByEmailAsync(dto.Email);
            if (u == null || !await _users.CheckPasswordAsync(u, dto.Password))
                return Unauthorized();

            if (!await _users.IsEmailConfirmedAsync(u))
                return Unauthorized("Email neconfirmat.");

            var access = CreateAccessToken(u, TimeSpan.FromHours(12));
            var refresh = Guid.NewGuid().ToString("N"); // pentru început; ideal: tabel RefreshTokens

            // TODO: salvează refresh token în DB cu expirație/rotație
            return new TokenResponse(access.TokenString, refresh, access.ExpiresUtc);
        }

        // minimal; poți extinde cu rotație, revocare etc.
        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponse>> Refresh(string refreshToken, string email)
        {
            var u = await _users.FindByEmailAsync(email);
            if (u == null) return Unauthorized();
            // TODO: validează refreshToken din DB
            var access = CreateAccessToken(u, TimeSpan.FromHours(12));
            var newRefresh = Guid.NewGuid().ToString("N");
            // TODO: invalidează vechiul refresh + salvează pe cel nou
            return new TokenResponse(access.TokenString, newRefresh, access.ExpiresUtc);
        }

        private (string TokenString, DateTime ExpiresUtc) CreateAccessToken(AppUser u, TimeSpan lifetime)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, u.Id),
            new Claim(JwtRegisteredClaimNames.Email, u.Email!),
            new Claim("customerNumber", u.CustomerNumber ?? ""),
            new Claim("parkingContractId", u.ParkingContractId?.ToString() ?? "")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(lifetime);

            var jwt = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}
