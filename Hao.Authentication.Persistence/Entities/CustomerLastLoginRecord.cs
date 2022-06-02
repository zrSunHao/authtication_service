using Hao.Authentication.Persistence.Attributes;
using Hao.Authentication.Persistence.Consts;
using Hao.Authentication.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [TablePrefix(PrefixConsts.UserLastLoginRecord)]
    [Table(nameof(UserLastLoginRecord))]
    [Index(nameof(CustomerId), IsUnique = false)]
    [Index(nameof(SysId), IsUnique = false)]
    [Index(nameof(RoleId), IsUnique = false)]
    public class UserLastLoginRecord : BaseInfo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(32)]
        public string SysId { get; set; }

        [Required]
        [MaxLength(32)]
        public string RoleId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }
    }
}
