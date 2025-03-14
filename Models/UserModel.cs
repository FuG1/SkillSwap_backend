using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSwap.Models
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [Required]
        [Column(TypeName = "varchar(64)")]
        [StringLength(64)]
        public required string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(64)")]
        [StringLength(64)]
        [EmailAddress]
        public required string Email { get; set; }

        [Column(TypeName = "varchar(1024)")]
        [StringLength(1024)]
        public string? About { get; set; } = null;

        [Required]
        [Column(TypeName = "char(60)")]
        [StringLength(60)]
        public required string PasswordHash { get; set; }
    }
}