using Microsoft.EntityFrameworkCore;
using SkillSwap.Contexts;
using SkillSwap.Models;

namespace SkillSwap.Services
{
    public interface IAuthService
    {
        public Task<string?> RegisterAsync(string firstName, string lastName, string email, string password);
        public Task<string?> LoginAsync(string email, string password);
    }

    public class AuthService(ApplicationContext context, IPasswordService passwordService, IJwtService jwtService) : IAuthService
    {
        private readonly ApplicationContext _context = context;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<string?> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return null;
            }

            var user = new UserModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = _passwordService.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "LastName", user.LastName }
            };

            return _jwtService.GenerateToken(user.Id.ToString(), claims);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            var claims = new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "LastName", user.LastName }
            };

            return _jwtService.GenerateToken(user.Id.ToString(), claims);
        }
    }
}