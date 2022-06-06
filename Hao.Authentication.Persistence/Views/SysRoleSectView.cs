using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class SysRoleSectView
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string SysId { get; set; }

        public string SysCode { get; set; }

        public string PgmId { get; set; }

        public string PgmCode { get; set; }

        public string SectId { get; set; }

        public string SectName { get; set; }

        public string SectCode { get; set; }

        public SectionCategory Category { get; set; }
    }
}
