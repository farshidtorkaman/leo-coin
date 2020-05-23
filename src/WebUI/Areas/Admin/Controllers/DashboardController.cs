using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class DashboardController : ControllerBase
    {
        [Route("dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}