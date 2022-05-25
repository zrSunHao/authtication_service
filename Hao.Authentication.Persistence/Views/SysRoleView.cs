using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class SysRoleView
    {
        public string Id { get; set; }

        public SysRoleRank Rank { get; set; }

        public string Name { get; set; }

        public string SystemId { get; set; }

        public ConstraintMethod LimitedMethod { get; set; }

        public DateTime? LimitedExpiredAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Remark { get; set; }
    }
}
