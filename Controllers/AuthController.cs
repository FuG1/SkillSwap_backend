using System.Net.Mail;
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] ILoginData loginData)
        {
            if (!IsValidEmail(loginData.Email))
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Некорректный формат почты"
                });
            }

            if (string.IsNullOrEmpty(loginData.Password) || loginData.Password.Length < 8)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Пароль должен содержать не менее 8 символов"
                });
            }

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
            if (!IsValidEmail(registerData.Email))
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Некорректный формат почты"
                });
            }

            if (string.IsNullOrEmpty(registerData.Password) || registerData.Password.Length < 8)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "Пароль должен содержать не менее 8 символов"
                });
            }

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
