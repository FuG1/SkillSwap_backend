using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillSwap.Models
{
    [Table("skills")]
    public class SkillModel : BaseModel
    {
        [Required]
        [ForeignKey("User")]
        public required int UserId { get; set; }

        [Required]
        [Column(TypeName = "varchar(64)")]
        [StringLength(64)]
        public required string Name { get; set; }

        public virtual UserModel? User { get; set; }
    }
}
