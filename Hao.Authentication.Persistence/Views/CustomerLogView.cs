using Hao.Authentication.Common.Enums;

namespace Hao.Authentication.Persistence.Views
{
    public class CustomerLogView
    {
        public long Id { get; set; }

        public string CtmId { get; set; }

        public string CtmName { get; set; }

        public string CtmNickname { get; set; }

        public string Operate { get; set; }

        public string? SysId { get; set; }

        public string? SysCode { get; set; }

        public string? SysName { get; set; }

        public string? PgmId { get; set; }

        public string? PgmName { get; set; }

        public string? PgmCode { get; set; }

        public string? RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? RoleCode { get; set; }

        public SysRoleRank? RoleRank { get; set; }

        public string? RemoteAddress { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
