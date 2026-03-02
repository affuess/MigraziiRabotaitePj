using Microsoft.AspNetCore.Identity;

namespace MigraziiRabotaitePj.Auth
{
    public class AppUser : IdentityUser<Guid>
    {
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
