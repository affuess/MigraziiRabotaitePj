using MigraziiRabotaitePj.Auth;

namespace MigraziiRabotaitePj.Services
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(AppUser user);
    }
}
