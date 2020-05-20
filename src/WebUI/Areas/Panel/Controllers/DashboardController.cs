using System.Threading.Tasks;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        [Route("dashboard")]
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserService.UserId;
            return View(await Mediator.Send(new GetConfirmationReportQuery {UserId = userId}));
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