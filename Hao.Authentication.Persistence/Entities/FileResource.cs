using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Entities
{
    [Table(nameof(FileResource))]
    [Index(nameof(OwnId), IsUnique = false)]
    public class FileResource
    {
        [Key]
        [MaxLength(32)]
        public string Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string OwnId { get; set; }

        [MaxLength(16)]
        public string Category { get; set; }

        [MaxLength(64)]
        public string FileName { get; set; }

        [MaxLength(64)]
        public string Type { get; set; }

        [MaxLength(16)]
        public string Suffix { get; set; }

        [Required]
        public long Length { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
