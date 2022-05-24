using Hao.Authentication.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(Constraint))]
    [Index(nameof(TargetId), IsUnique = true)]
    public class Constraint
    {
        [Key]
        [MaxLength(32)]
        public string Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string TargetId { get; set; }

        [Required]
        public ConstraintCategory Category { get; set; }

        [Required]
        public ConstraintMethod Method { get; set; }

        [Required]
        [MaxLength(256)]
        public string Origin { get; set; }

        [Required]
        [MaxLength(256)]
        public string Remark { get; set; }

        public DateTime? ExpiredAt { get; set; }

        [Required]
        public bool Cancelled { get; set; } = false;
    }
}
