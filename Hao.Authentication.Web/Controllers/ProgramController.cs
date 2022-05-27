using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramManager _manager;

        public ProgramController(IProgramManager manager)
        {
            _manager = manager;
        }


        [HttpPost(Name = "Add")]
        public async Task<ResponseResult<bool>> Add(ProgramM model)
        {
            return await _manager.Add(model);
        }
    }
}
