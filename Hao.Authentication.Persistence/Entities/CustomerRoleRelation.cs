using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(CustomerRoleRelation))]
    [Index(nameof(CustomerId), IsUnique = false)]
    [Index(nameof(RoleId), IsUnique = false)]
    public class CustomerRoleRelation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string CustomerId { get; set; }

        [MaxLength(32)]
        [Required]
        public string SysId { get; set; }

        [MaxLength(32)]
        [Required]
        public string RoleId { get; set; }

        [MaxLength(256)]
        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        [MaxLength(32)]
        public string? CreatedById { get; set; }
    }
}
