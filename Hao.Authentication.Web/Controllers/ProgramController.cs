﻿using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
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

        [HttpPost("Add")]
        public async Task<ResponseResult<bool>> Add([FromBody]ProgramM model)
        {
            return await _manager.Add(model);
        }

        [HttpPatch("Update")]
        public async Task<ResponseResult<bool>> Update(ProgramM model)
        {
            return await _manager.Update(model);
        }

        [HttpDelete("Delete")]
        public async Task<ResponseResult<bool>> Delete(string id)
        {
            return await _manager.Delete(id);
        }

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



        [HttpPost("AddSect")]
        public async Task<ResponseResult<bool>> AddSect(SectM model)
        {
            return await _manager.AddSect(model);
        }

        [HttpPatch("UpdateSect")]
        public async Task<ResponseResult<bool>> UpdateSect(SectM model)
        {
            return await _manager.UpdateSect(model);
        }

        [HttpDelete("DeleteSect")]
        public async Task<ResponseResult<bool>> DeleteSect(string id)
        {
            return await _manager.DeleteSect(id);
        }

        [HttpGet("GetSectList")]
        public async Task<ResponsePagingResult<SectM>> GetSectList(string pgmId)
        {
            return await _manager.GetSectList(pgmId);
        }



        [HttpPost("AddFunct")]
        public async Task<ResponseResult<bool>> AddFunct(FunctM model)
        {
            return await _manager.AddFunct(model);
        }

        [HttpPatch("UpdateFunct")]
        public async Task<ResponseResult<bool>> UpdateFunct(FunctM model)
        {
            return await _manager.UpdateFunct(model);
        }

        [HttpDelete("DeleteFunct")]
        public async Task<ResponseResult<bool>> DeleteFunct(string id)
        {
            return await _manager.DeleteFunct(id);
        }

        [HttpGet("GetFunctList")]
        public async Task<ResponsePagingResult<FunctM>> GetFunctList(string sectId)
        {
            return await _manager.GetFunctList(sectId);
        }


        [HttpPost("GetOwnedCtmList")]
        public async Task<ResponsePagingResult<PgmCtmM>> GetOwnedCtmList(PagingParameter<PgmCtmFilter> param)
        {
            return await _manager.GetOwnedCtmList(param);
        }

        [HttpPost("GetNotOwnedCtmList")]
        public async Task<ResponsePagingResult<PgmCtmM>> GetNotOwnedCtmList(PagingParameter<PgmCtmFilter> param)
        {
            return await _manager.GetNotOwnedCtmList(param);
        }

        [HttpPost("AddCtm")]
        public async Task<ResponseResult<bool>> AddCtm(string pgmId, string ctmId)
        {
            return await _manager.AddCtm(pgmId, ctmId);
        }

        [HttpDelete("DeleteCtm")]
        public async Task<ResponseResult<bool>> DeleteCtm(string pgmId, string ctmId)
        {
            return await _manager.DeleteCtm(pgmId, ctmId);
        }
    }
}
