using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{
    public class CttM
    {
        public string Id { get; set; }

        public string CtmId { get; set; }

        public string CtmName { get; set; }

        public string SysId { get; set; }

        public string SysName { get; set; }

        public string FunctId { get; set; }

        public string FunctName { get; set; }

        public ConstraintCategory Category { get; set; }

        public ConstraintMethod Method { get; set; }

        public string Origin { get; set; }

        public string Remark { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CttFilter
    {
        public string Name { get; set; }

        public ConstraintCategory? Category { get; set; }

        public string OriginOrRemark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
