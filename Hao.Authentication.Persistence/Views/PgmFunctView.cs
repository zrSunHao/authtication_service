using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class PgmFunctView
    {
        public string Id { get; set; }

        public string PgmId { get; set; }

        public string SectId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ConstraintMethod? CttMethod { get; set; }

        public DateTime? LimitedExpiredAt { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
