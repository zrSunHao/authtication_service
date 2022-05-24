using Hao.Authentication.Common.Enums;
using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    public class Program : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public ProgramCategory Category { get; set; }

        public string Remark { get; set; }
    }
}
