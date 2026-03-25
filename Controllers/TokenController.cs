using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MigraziiRabotaitePj.Auth;
using MigraziiRabotaitePj.Services;
using Microsoft.AspNetCore.Identity;

namespace MigraziiRabotaitePj.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public TokenController(ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }


        [HttpPost("generate")]
        public async Task<IActionResult> GenerateToken([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found");

            var token = await _tokenService.CreateAccessTokenAsync(user);
            return Ok(new { token });
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            var token = await _tokenService.CreateAccessTokenAsync(user);
            return Ok(new { token });
        }


        [HttpPost("revoke")]
        [Authorize]
        public IActionResult RevokeToken()
        {
            return Ok("Token revoked");
        }
    }
}
