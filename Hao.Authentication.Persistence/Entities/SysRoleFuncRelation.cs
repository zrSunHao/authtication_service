using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(SysRoleFuncRelation))]
    [Index(nameof(RoleId), IsUnique = false)]
    [Index(nameof(FuncId), IsUnique = false)]
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
        public string FuncId { get; set; }
    }
}
