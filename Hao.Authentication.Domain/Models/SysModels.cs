using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{
    public class SysM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Code { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class SysProgramM
    {
        public string Id { get; set; }

        public string SysId { get; set; }

        public string Name { get; set; }

        public ProgramCategory Category { get; set; }

        public string Code { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class SysRoleM
    {
        public string Id { get; set; }

        public SysRoleRank Rank { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string SysId { get; set; }

        public string Intro { get; set; }

        public string Remark { get; set; }

        public ConstraintMethod? CttMethod { get; set; }

        public DateTime? LimitedExpiredAt { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class SysRoleFunctM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PgmId { get; set; }

        public string SectId { get; set; }

        public bool Checked { get; set; } = false;

    }

    public class SysRoleSectM
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string PgmId { get; set; }

        public bool Checked { get; set; } = false;

        public List<SysRoleFunctM> Functs { get; set; }
    }

    public class SysRolePgmM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<SysRoleSectM> Sects { get; set; }
    }

    public class SysRoleRelationM
    {
        public string RoleId { get; set; }

        public string PgmId { get; set; }

        public List<string> SectIds { get; set; }

        public List<string> FuncIds { get; set; }
    }

    public class SysCtmM
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public bool Limited { get; set; }

        public string RoleId { get; set; }

        public SysRoleRank RoleRank { get; set; }

        public string RoleName { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }



    public class SysFilter
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string IntroOrRemark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }

    public class SysOwnedPgmFilter
    {
        public string SysId { get; set; }

        public string NameOrCode { get; set; }

        public ProgramCategory? Category { get; set; }
    }

    public class SysNotOwnedPgmFilter
    {
        public string SysId { get; set; }

        public string NameOrCode { get; set; }

        public ProgramCategory? Category { get; set; }

        public string IntroOrRemark { get; set; }
    }

    public class SysRoleFilter
    {
        public string SysId { get; set; }

        public string NameOrCode { get; set; }

        public ConstraintMethod? CttMethod { get; set; }

        public SysRoleRank? Rank { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }

    public class SysCtmFilter
    {
        public string SysId { get; set; }

        public string NameOrRole { get; set; }

        public bool? Limited { get; set; }

        public string Remark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
