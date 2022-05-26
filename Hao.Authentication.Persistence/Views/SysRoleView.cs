using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class SysRoleView
    {
        public string Id { get; set; }

        public SysRoleRank Rank { get; set; }

        public string Name { get; set; }

        public string SysId { get; set; }

        public string SysName { get; set; }

        public string SysCode { get; set; }

        public bool Limited { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Remark { get; set; }
    }
}
