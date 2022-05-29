using Hao.Authentication.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hao.Authentication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConstraintController : ControllerBase
    {
        private readonly IConstraintManager _manager;

        public ConstraintController(IConstraintManager manager)
        {
            _manager = manager;
        }

    }
}
