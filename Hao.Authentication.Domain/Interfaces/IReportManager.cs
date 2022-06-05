using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;

namespace Hao.Authentication.Domain.Interfaces
{
    public interface IReportManager
    {
        public Task<ResponsePagingResult<WidgetM>> GetWidgetList();

        public Task<ResponsePagingResult<RecentLoginCtmM>> GetRecentLoginCtmList();

        public Task<ResponsePagingResult<RecentLogM>> GetRecentLogList();

        public Task<ResponsePagingResult<CttM>> GetCtts();
    }
}
