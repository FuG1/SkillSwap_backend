using SkillSwap.Models;
using SkillSwap.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkillSwap.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<string?> RegisterAsync(string username, string email, string password);
    }

    public class AuthService(IUserService userService, IJwtService jwtService) : IAuthService
    {
        private readonly IUserService _userService = userService;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<string?> LoginAsync(string login, string password)
        {
            var user = await _userService.GetUserByEmailAsync(login);
            if (user == null)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
            return _jwtService.GenerateToken(new ITokenPayload
            {
                UserId = user.Id
            });
        }

        public async Task<string?> RegisterAsync(string username, string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                return null;
            }
            var newUser = await _userService.CreateUserAsync(new UserModel
            {
                UserName = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            });
            return _jwtService.GenerateToken(new ITokenPayload
            {
                UserId = newUser.Id
            });
        }
    }
}
