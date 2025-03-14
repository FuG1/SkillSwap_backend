using SkillSwap.Contexts;
using SkillSwap.Models;
using Microsoft.EntityFrameworkCore;

namespace SkillSwap.Services
{
    public interface IUserService
    {
        public Task<UserModel?> GetUserByIdAsync(int id);
        public Task<UserModel?> GetUserByEmailAsync(string email);
        public Task<UserModel> CreateUserAsync(UserModel user);
        public Task<UserModel> UpdateUserAsync(UserModel user);
        public Task<UserModel> RemoveUserAsync(UserModel user);
    }

    public class UserService(ApplicationContext context) : IUserService
    {
        private readonly ApplicationContext _context = context;

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(model => model.Id == id);
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(model => model.Email == email);
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            var user = await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
        public async Task<UserModel> UpdateUserAsync(UserModel userModel)
        {
            var user = _context.Users.Update(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
        public async Task<UserModel> RemoveUserAsync(UserModel userModel)
        {
            var user = _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
    }
}