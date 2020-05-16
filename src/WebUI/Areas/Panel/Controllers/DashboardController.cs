using System.Threading.Tasks;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Reports.Queries;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("dashboard")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await Mediator.Send(new GetConfirmationReportQuery {UserId = user.Id}));
        }

        [Route("second")]
        public IActionResult Second()
        {
            return View();
        }

        [HttpPost]
        [Route("load_currencies")]
        public async Task<IActionResult> LoadCurrencies()
        {
            return Json(await Mediator.Send(new LoadCurrenciesQuery()));
        }
    }
}