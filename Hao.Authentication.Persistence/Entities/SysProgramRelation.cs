using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(SysProgramRelation))]
    [Index(nameof(SysId), IsUnique = false)]
    [Index(nameof(ProgramId), IsUnique = false)]
    public class SysProgramRelation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string SysId { get; set; }

        [MaxLength(32)]
        [Required]
        public string ProgramId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
