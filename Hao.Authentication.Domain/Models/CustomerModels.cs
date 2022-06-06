using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Domain.Models
{

    public class CtmM
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public bool Limited { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? Remark { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class CtmRoleM
    {
        public string Id { get; set; }

        public string SysId { get; set; }

        public string? SysLogo { get; set; }

        public string SysName { get; set; }

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public SysRoleRank Rank { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CtmLogM
    {
        public long Id { get; set; }

        public string Operate { get; set; }

        public string SysName { get; set; }

        public string PgmName { get; set; }

        public string? RoleName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Remark { get; set; }
    }

    public class PeopleM
    {
        public string CtmId { get; set; }

        public string FullName { get; set; }

        public CustomerGender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public CustomerEducation Education { get; set; }

        public string Profession { get; set; }

        public string Intro { get; set; }

        public DateTime? LastModifiedAt { get; set; }
    }

    public class CtmCttAddM
    {
        public string CtmId { get; set; }

        public ConstraintCategory Category { get; set; }

        public ConstraintMethod Method { get; set; }

        public string? SysId { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public string Remark { get; set; }
    }

    public class CtmRoleUpdateM
    {
        public string CtmId { get; set; }

        public string SysId { get; set; }

        public string RoleId { get; set; }
    }



    public class CtmFilter
    {
        public string NameOrNickname { get; set; }

        public bool? Limited { get; set; }

        public string Remark { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }

    public class CtmRoleFilter
    {
        public string CtmId { get; set; }

        public string SysName { get; set; }

        public string RoleName { get; set; }
    }

    public class CtmCttFilter
    {
        public string CtmId { get; set; }

        public ConstraintCategory? Category { get; set; }

        public string SysName { get; set; }
    }

    public class CtmLogFilter
    {
        public string CtmId { get; set; }

        public string Operate { get; set; }

        public string SysName { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }
    }
}
