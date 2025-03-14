using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSwap.Models
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public required string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(256)")]
        [StringLength(256)]
        [EmailAddress]
        public required string Email  { get; set; }

        [Required]
        [Column(TypeName = "varchar(256)")]
        [StringLength(256)]
        public required string PasswordHash { get; set; }
    }
}