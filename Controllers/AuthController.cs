using SkillSwap.Interfaces.Options;
using SkillSwap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.Data;
using SkillSwap.Interfaces;

namespace SkillSwap.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICookieOptions _cookieOptions;

        public AuthController(IAuthService authService, IOptions<ICookieOptions> cookieOptions)
        {
            _authService = authService;
            _cookieOptions = cookieOptions.Value;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] ILoginData loginData)
        {
            var token = await _authService.LoginAsync(loginData.Email, loginData.Password);
            if (token == null)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Неверное имя пользователя или пароль"
                });
            }
            HttpContext.Response.Cookies.Append(_cookieOptions.JwtToken, token, new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(_cookieOptions.Expiration),
                Domain = _cookieOptions.Domain,
                HttpOnly = true
            });
            return Ok(new { Status = "ok" });
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] IRegisterData registerData)
        {
            var token = await _authService.RegisterAsync(registerData.UserName, registerData.Email, registerData.Password);
            if (token == null)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Такой пользователь уже существует"
                });
            }
            HttpContext.Response.Cookies.Append(_cookieOptions.JwtToken, token, new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(_cookieOptions.Expiration),
                Domain = _cookieOptions.Domain,
                HttpOnly = true
            });
            return Ok(new { Status = "ok" });
        }
    }
}
