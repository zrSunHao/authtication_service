using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IProgramManager
    {
        public Task<ResponseResult<bool>> Add(ProgramM model);

        public Task<ResponseResult<bool>> Update(ProgramM model);

        public Task<ResponseResult<bool>> Delete(string id);

        public Task<ResponsePagingResult<ProgramM>> GetList(PagingParameter<PgmFilter> param);



        public Task<ResponseResult<bool>> AddSect(SectM model);

        public Task<ResponseResult<bool>> UpdateSect(SectM model);

        public Task<ResponseResult<bool>> DeleteSect(string id);

        public Task<ResponsePagingResult<SectM>> GetSectList(string pgmId);



        public Task<ResponseResult<bool>> AddFunct(FunctM model);

        public Task<ResponseResult<bool>> UpdateFunct(FunctM model);

        public Task<ResponseResult<bool>> DeleteFunct(string id);

        public Task<ResponsePagingResult<FunctM>> GetFunctList(string sectId);



        public Task<ResponsePagingResult<PgmCtmM>> GetOwnedCtmList(PagingParameter<PgmCtmFilter> param);

        public Task<ResponsePagingResult<PgmCtmM>> GetNotOwnedCtmList(PagingParameter<PgmCtmFilter> param);

        public Task<ResponseResult<bool>> AddCtm(string pgmId, string ctmId);

        public Task<ResponseResult<bool>> DeleteCtm(string pgmId, string ctmId);
    }
}
