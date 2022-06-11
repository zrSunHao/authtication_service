using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hao.Authentication.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramManager _manager;

        public ProgramController(IProgramManager manager)
        {
            _manager = manager;
        }

        [Function(PgmFunct.Add)]
        [HttpPost("Add")]
        public async Task<ResponseResult<bool>> Add([FromBody]ProgramM model)
        {
            return await _manager.Add(model);
        }

        [Function(PgmFunct.Update)]
        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(ProgramM model)
        {
            return await _manager.Update(model);
        }

        [Function(PgmFunct.Delete)]
        [HttpDelete("Delete")]
        public async Task<ResponseResult<bool>> Delete(string id)
        {
            return await _manager.Delete(id);
        }

        [Function(PgmFunct.GetList)]
        [HttpPost("GetList")]
        public async Task<ResponsePagingResult<ProgramM>> GetList(PagingParameter<PgmFilter> param)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            var res =  await _manager.GetList(param);
            watch1.Stop();
            Console.WriteLine($"controller耗时{watch1.ElapsedMilliseconds}毫秒");
            return res;
        }

        [HttpPatch("FlushCache")]
        public async Task<ResponseResult<bool>> FlushCache(string id)
        {
            return await _manager.FlushCache(id);
        }



        [Function(PgmFunct.AddSect)]
        [HttpPost("AddSect")]
        public async Task<ResponseResult<bool>> AddSect(SectM model)
        {
            return await _manager.AddSect(model);
        }

        [Function(PgmFunct.UpdateSect)]
        [HttpPatch("UpdateSect")]
        public async Task<ResponseResult<bool>> UpdateSect(SectM model)
        {
            return await _manager.UpdateSect(model);
        }

        [Function(PgmFunct.DeleteSect)]
        [HttpDelete("DeleteSect")]
        public async Task<ResponseResult<bool>> DeleteSect(string id)
        {
            return await _manager.DeleteSect(id);
        }

        [Function(PgmFunct.GetSectList)]
        [HttpGet("GetSectList")]
        public async Task<ResponsePagingResult<SectM>> GetSectList(string pgmId)
        {
            return await _manager.GetSectList(pgmId);
        }



        [Function(PgmFunct.AddFunct)]
        [HttpPost("AddFunct")]
        public async Task<ResponseResult<bool>> AddFunct(FunctM model)
        {
            return await _manager.AddFunct(model);
        }

        [Function(PgmFunct.UpdateFunct)]
        [HttpPatch("UpdateFunct")]
        public async Task<ResponseResult<bool>> UpdateFunct(FunctM model)
        {
            return await _manager.UpdateFunct(model);
        }

        [Function(PgmFunct.DeleteFunct)]
        [HttpDelete("DeleteFunct")]
        public async Task<ResponseResult<bool>> DeleteFunct(string id)
        {
            return await _manager.DeleteFunct(id);
        }

        [Function(PgmFunct.GetFunctList)]
        [HttpGet("GetFunctList")]
        public async Task<ResponsePagingResult<FunctM>> GetFunctList(string sectId)
        {
            return await _manager.GetFunctList(sectId);
        }


        [Function(PgmFunct.GetOwnedCtmList)]
        [HttpPost("GetOwnedCtmList")]
        public async Task<ResponsePagingResult<PgmCtmM>> GetOwnedCtmList(PagingParameter<PgmCtmFilter> param)
        {
            return await _manager.GetOwnedCtmList(param);
        }

        [Function(PgmFunct.GetNotOwnedCtmList)]
        [HttpPost("GetNotOwnedCtmList")]
        public async Task<ResponsePagingResult<PgmCtmM>> GetNotOwnedCtmList(PagingParameter<PgmCtmFilter> param)
        {
            return await _manager.GetNotOwnedCtmList(param);
        }

        [Function(PgmFunct.AddCtm)]
        [HttpPost("AddCtm")]
        public async Task<ResponseResult<bool>> AddCtm(string pgmId, string ctmId)
        {
            return await _manager.AddCtm(pgmId, ctmId);
        }

        [Function(PgmFunct.DeleteCtm)]
        [HttpDelete("DeleteCtm")]
        public async Task<ResponseResult<bool>> DeleteCtm(string pgmId, string ctmId)
        {
            return await _manager.DeleteCtm(pgmId, ctmId);
        }
    }
}
