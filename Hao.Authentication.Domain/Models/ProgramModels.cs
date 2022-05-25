﻿using Hao.Authentication.Common.Enums;

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

    public class SectElet
    {
        public string Id { get; set; }

        public string ProgramId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Remark { get; set; }
    }

    public class FunctElet
    {
        public string Id { get; set; }

        public string ProgramId { get; set; }

        public string SectionId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ConstraintCategory CttCategory { get; set; }

        public DateTime? LimitedExpiredAt { get; set; }

        public string Eemark { get; set; }
    }

    public class PgmFilter
    {
        public string NameOrCode { get; set; }

        public ProgramCategory Category { get; set; }

        public string IntroOrRemark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
