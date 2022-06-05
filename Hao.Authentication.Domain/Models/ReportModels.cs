namespace Hao.Authentication.Domain.Models
{
    public class WidgetM
    {
        public string Msg { get; set; }

        public string Icon { get; set; }
    }

    public class RecentLoginCtmM
    {
        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string SysName { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }

    public class RecentLogM
    {
        public string Name { get; set; }

        public string RoleName { get; set; }

        public string Operate { get; set; }

        public string SysName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
