using MigraziiRabotaitePj.Auth;

namespace MigraziiRabotaitePj.Services
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(AppUser user);
        Task<string> CreateRefreshTokenAsync(AppUser user);
        Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
