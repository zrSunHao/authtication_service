using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(CustomerLog))]
    [Index(nameof(CustomerId), IsUnique = false)]
    public class CustomerLog
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Operate { get; set; }

        [MaxLength(32)]
        public string? SystemId { get; set; }

        [MaxLength(32)]
        public string ProgramId { get; set; }

        [Required]
        [MaxLength(32)]
        public string? RoleId { get; set; }

        [MaxLength(64)]
        public string RemoteAddress { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(512)]
        public string Remark { get; set; }
    }
}
