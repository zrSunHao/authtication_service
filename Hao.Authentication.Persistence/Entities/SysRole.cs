using Hao.Authentication.Common.Enums;
using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    public class SysRole : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public SysRoleRank Rank { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }
    }
}
