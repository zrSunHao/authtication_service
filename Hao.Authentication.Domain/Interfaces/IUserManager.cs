using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IUserManager
    {
        public Task<ResponseResult<AuthResultM>> Login(LoginM model);

        public Task<ResponseResult<bool>> Register(RegisterM model);

        public Task<ResponseResult<AuthResultM>> ResetPsd(string oldPsd,string newPsd);
    }
}
