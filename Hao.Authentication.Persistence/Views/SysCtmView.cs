using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class SysCtmView
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public bool Limited { get; set; }

        public string SysId { get; set; }

        public string SysName { get; set; }

        public string RoleId { get; set; }

        public SysRoleRank RoleRank { get; set; }

        public string RoleName { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
