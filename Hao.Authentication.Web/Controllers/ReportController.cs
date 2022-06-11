using Hao.Authentication.Domain.Consts;
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

        [Function(ReportFunct.GetWidgetList)]
        [HttpGet("GetWidgetList")]
        public async Task<ResponsePagingResult<WidgetM>> GetWidgetList()
        {
            return await _manager.GetWidgetList();
        }

        [Function(ReportFunct.GetRecentLoginCtmList)]
        [HttpGet("GetRecentLoginCtmList")]
        public async Task<ResponsePagingResult<RecentLoginCtmM>> GetRecentLoginCtmList()
        {
            return await _manager.GetRecentLoginCtmList();
        }

        [Function(ReportFunct.GetRecentLogList)]
        [HttpGet("GetRecentLogList")]
        public async Task<ResponsePagingResult<RecentLogM>> GetRecentLogList()
        {
            return await _manager.GetRecentLogList();
        }

        [Function(ReportFunct.GetCtts)]
        [HttpGet("GetCtts")]
        public async Task<ResponsePagingResult<CttM>> GetCtts()
        {
            return await _manager.GetCtts();
        }
    }
}
