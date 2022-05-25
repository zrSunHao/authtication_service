using Hao.Authentication.Persistence.Database;

namespace Hao.Authentication.Persistence.Entities
{
    public class ProgramFunction : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string SectionId { get; set; }

        public string ProgramId { get; set; }

        public string Remark { get; set; }
    }
}
