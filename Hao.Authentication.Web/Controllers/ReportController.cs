using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportManager _manager;

        public ReportController(IReportManager manager)
        {
            _manager = manager;
        }

        [Function("Report_GetWidgetList")]
        [HttpGet("GetWidgetList")]
        public async Task<ResponsePagingResult<WidgetM>> GetWidgetList()
        {
            return await _manager.GetWidgetList();
        }

        [HttpGet("GetRecentLoginCtmList")]
        public async Task<ResponsePagingResult<RecentLoginCtmM>> GetRecentLoginCtmList()
        {
            return await _manager.GetRecentLoginCtmList();
        }

        [HttpGet("GetRecentLogList")]
        public async Task<ResponsePagingResult<RecentLogM>> GetRecentLogList()
        {
            return await _manager.GetRecentLogList();
        }

        [HttpGet("GetCtts")]
        public async Task<ResponsePagingResult<CttM>> GetCtts()
        {
            return await _manager.GetCtts();
        }
    }
}
