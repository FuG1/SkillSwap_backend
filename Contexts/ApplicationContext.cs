using Microsoft.EntityFrameworkCore;
using SkillSwap.Models;

namespace SkillSwap.Contexts
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public required DbSet<UserModel> Users { get; set; }
        public required DbSet<SkillModel> Skills { get; set; }
        public required DbSet<LearnModel> Learns { get; set; }
    }
}