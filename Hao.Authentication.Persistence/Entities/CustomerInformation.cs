using Hao.Authentication.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{

    [Table(nameof(CustomerInformation))]
    [Index(nameof(CustomerId), IsUnique = true)]
    public class CustomerInformation
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string CustomerId { get; set; }

        [MaxLength(32)]
        public string FullName { get; set; }

        public CustomerGender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public CustomerEducation Education { get; set; }

        [MaxLength(64)]
        public string Profession { get; set; }

        [MaxLength(256)]
        public string Intro { get; set; }

        [Required]
        public DateTime LastModifiedAt { get; set; }
    }
}
