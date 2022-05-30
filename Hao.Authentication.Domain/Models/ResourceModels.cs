namespace Hao.Authentication.Domain.Models
{
    public class ResourceM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string OwnId { get; set; }

        public string Category { get; set; }

        public string FileName { get; set; }

        public string? Type { get; set; }

        public string? Suffix { get; set; }

        public long Length { get; set; }
    }
}
