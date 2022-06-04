using Hao.Authentication.Common.Enums;
using Hao.Authentication.Persistence.Attributes;
using Hao.Authentication.Persistence.Consts;
using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Constraint)]
    [Table(nameof(Constraint))]
    [Index(nameof(TargetId), IsUnique = false)]
    public class Constraint : BaseInfo
    {
        [Key]
        [MaxLength(32)]
        public string Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string TargetId { get; set; } // customer/role/function

        [MaxLength(32)]
        public string? SysId { get; set; }

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

        public DateTime CreatedAt { get; set; }

        [MaxLength(32)]
        public string? CreatedById { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        [MaxLength(32)]
        public string? LastModifiedById { get; set; }
    }
}
