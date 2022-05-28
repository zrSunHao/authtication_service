using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{
    public class ProgramM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ProgramCategory Category { get; set; }

        public string Code { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class SectM
    {
        public string Id { get; set; }

        public string PgmId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Remark { get; set; }
    }

    public class FunctM
    {
        public string Id { get; set; }

        public string PgmId { get; set; }

        public string SectId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ConstraintMethod? CttMethod { get; set; }

        public DateTime? LimitedExpiredAt { get; set; }

        public string Remark { get; set; }
    }




    public class PgmFilter
    {
        public string NameOrCode { get; set; }

        public ProgramCategory? Category { get; set; }

        public string Remark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
