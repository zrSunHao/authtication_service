using Hao.Authentication.Common.Enums;
using Hao.Authentication.Persistence.Attributes;
using Hao.Authentication.Persistence.Consts;
using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    [TablePrefix(PrefixConsts.ProgramSection)]
    public class ProgramSection : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string ProgramId { get; set; }

        public SectionCategory Category { get; set; }

        public string Remark { get; set; }
    }
}
