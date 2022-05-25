using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{
    public class CttFilter
    {
        public string CustomerName { get; set; }

        public ConstraintCategory? Category { get; set; }

        public string OriginOrRemark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
