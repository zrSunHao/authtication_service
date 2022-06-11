using Hao.Authentication.Domain.Models;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IPrivilegeManager
    {
        public Task<UserLastLoginRecordM> GetLoginRecord(string? sid);

        public Task<List<string>> GetFunctCodes(string roleId);

        public Task<List<string>> GetPgmFunctCodes(string roleId, string pgmCode);

        public Task<List<string>> GetPgmSectCodes(string roleId, string pgmCode);

        public Task<SysRoleM> GetRoleById(string id);
    }
}
