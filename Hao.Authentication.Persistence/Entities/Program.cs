using Hao.Authentication.Common.Enums;
using Hao.Authentication.Persistence.Attributes;
using Hao.Authentication.Persistence.Consts;
using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    [TablePrefix(PrefixConsts.Program)]
    public class Program : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public ProgramCategory Category { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }
    }
}
