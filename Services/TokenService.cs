using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MigraziiRabotaitePj.Auth;
using MigraziiRabotaitePj.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MigraziiRabotaitePj.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataBase _context;

        public TokenService(IConfiguration configuration,
            UserManager<AppUser> userManager,
            DataBase context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> CreateAccessTokenAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshTokenAsync(AppUser user)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString("N"),
                ExpireAtUtc = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                User = user
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == token);
            if (refreshToken == null) return false;

            refreshToken.IsRevoke = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
