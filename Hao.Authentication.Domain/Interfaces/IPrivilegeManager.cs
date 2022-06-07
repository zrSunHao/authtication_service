using Hao.Authentication.Domain.Models;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IPrivilegeManager
    {
        public Task<UserLastLoginRecordM> GetLoginRecord(string? sid);

        public Task<List<string>> GetFunctCodes(string roleId);
    }
}
