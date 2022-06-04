using System.ComponentModel.DataAnnotations.Schema;

namespace Hao.Authentication.Persistence.Views
{
    public class CtmView
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public bool Limited { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
