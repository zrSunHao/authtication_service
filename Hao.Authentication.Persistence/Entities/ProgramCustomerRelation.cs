using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(ProgramCustomerRelation))]
    [Index(nameof(ProgramId), IsUnique = false)]
    [Index(nameof(CustomerId), IsUnique = false)]
    public class ProgramCustomerRelation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string ProgramId { get; set; }

        [Required]
        [MaxLength(32)]
        public string CustomerId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(32)]
        public string? CreatedById { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        [MaxLength(32)]
        public string? DeletedById { get; set; }
    }
}
