namespace Hao.Authentication.Domain.Models
{
    public class LogM
    {
        public string CustomerId { get; set; }

        public string Operate { get; set; }

        public string? SystemId { get; set; }

        public string ProgramId { get; set; }

        public string? RoleId { get; set; }

        public string? RemoteAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Remark { get; set; }
    }
}
