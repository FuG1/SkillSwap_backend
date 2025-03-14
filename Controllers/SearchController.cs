using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkillSwap.Attributes;
using SkillSwap.Contexts;
using SkillSwap.Interfaces;
using SkillSwap.Interfaces.Options;
using SkillSwap.Models;
using System.Threading.Tasks;

namespace SkillSwap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(ApplicationContext context, IOptions<ICookieOptions> cookieOptions) : ControllerBase

    {
        private readonly ICookieOptions cookieOptions = cookieOptions.Value;
        private readonly ApplicationContext _context = context;

        [Authorization]
        [HttpGet("matches")]
        public async Task<IActionResult> GetMatches()
        {
            var tokenPayload = HttpContext.Items[cookieOptions.JwtToken] as ITokenPayload;

            var mySkills = await _context.Skills
                .Where(s => s.UserId == tokenPayload.UserId)
                .Select(s => s.Name)
                .ToListAsync();
            var myLearns = await _context.Learns
                .Where(l => l.UserId == tokenPayload.UserId)
                .Select(l => l.Name)
                .ToListAsync();

            if (!mySkills.Any() || !myLearns.Any())
            {
                return BadRequest("У пользователя должны быть заданы и навыки, и направления для изучения.");
            }

            var matches = await _context.Users
                .Where(u => u.Id != tokenPayload.UserId &&
                    _context.Skills.Where(s => s.UserId == u.Id)
                                   .Select(s => s.Name)
                                   .Any(name => myLearns.Contains(name)) &&
                    _context.Learns.Where(l => l.UserId == u.Id)
                                   .Select(l => l.Name)
                                   .Any(name => mySkills.Contains(name)))
                .ToListAsync();

            return Ok(matches);
        }
    }
}
