using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(UserLastLoginRecord))]
    [Index(nameof(LoginId), IsUnique = true)]
    [Index(nameof(CustomerId), IsUnique = false)]
    [Index(nameof(SysId), IsUnique = false)]
    [Index(nameof(RoleId), IsUnique = false)]
    public class UserLastLoginRecord
    {
        [Key]
        public long Id { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LoginId { get; set; }

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
