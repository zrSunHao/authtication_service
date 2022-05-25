using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(SysRoleFuncRelation))]
    [Index(nameof(RoleId), IsUnique = false)]
    [Index(nameof(TargetId), IsUnique = false)]
    public class SysRoleFuncRelation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string RoleId { get; set; }

        [MaxLength(32)]
        [Required]
        public string ProgramId { get; set; }

        [MaxLength(32)]
        [Required]
        public string TargetId { get; set; }

        [Required]
        public bool IsFunction { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
