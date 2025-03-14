using Microsoft.AspNetCore.Mvc;
using SkillSwap.Interfaces;
using SkillSwap.Services;
using System.Threading.Tasks;

namespace SkillSwap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var token = await _authService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.Password);
            if (token == null)
            {
                return BadRequest("User with this email already exists.");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = token });
        }
    }
}