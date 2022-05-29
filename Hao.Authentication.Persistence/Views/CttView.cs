using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class CttView
    {
        public string Id { get; set; }

        public string? CtmId { get; set; }

        public string? CtmName { get; set; }

        public string? SysId { get; set; }

        public string? SysName { get; set; }

        public string? FunctId { get; set; }

        public string? FunctName { get; set; }

        public ConstraintCategory Category { get; set; }

        public ConstraintMethod Method { get; set; }

        public string? Origin { get; set; }

        public string? Remark { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
