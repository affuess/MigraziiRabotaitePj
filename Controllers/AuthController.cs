using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MigraziiRabotaitePj.Auth;
using MigraziiRabotaitePj.Services;

namespace MigraziiRabotaitePj.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Unauthorized();

            var check = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!check.Succeeded)
                return Unauthorized();

            var token = await _tokenService.CreateAccessTokenAsync(user);

            return Ok(new { token });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var token = await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            if (!token) return Unauthorized();

            return Ok("Token revoked");
        }

    }
}
