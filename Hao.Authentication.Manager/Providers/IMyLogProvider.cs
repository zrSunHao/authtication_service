using Hao.Authentication.Domain.Models;

namespace Hao.Authentication.Manager.Providers
{
    public interface IMyLogProvider
    {
        public void Add(UserLastLoginRecordM record, string operate, string remark = "", string? remote = "");

        public Task<int> Save(LogM log);
    }
}
