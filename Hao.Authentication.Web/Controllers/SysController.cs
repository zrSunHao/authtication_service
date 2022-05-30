using Hao.Authentication.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    public class SysController : ControllerBase
    {
        private readonly ISysManager _manager;

        public SysController(ISysManager manager)
        {
            _manager = manager;
        }
    }
}
