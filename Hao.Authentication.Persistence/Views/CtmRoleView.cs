using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class CtmRoleView
    {
        public string Id { get; set; }

        public string SysId { get; set; }

        public string? SysLogo { get; set; }

        public string SysName { get; set; }

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public SysRoleRank Rank { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
