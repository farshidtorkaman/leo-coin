using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Factors.Queries;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel/factors")]
    public class FactorsController : ControllerBase
    {
        [Route("buys")]
        public async Task<IActionResult> Purchases()
        {
            var userId = CurrentUserService.UserId;
            return View(await Mediator.Send(new GetPurchasedFactorsQuery {UserId = userId}));
        }

        [Route("sells")]
        public async Task<IActionResult> Sells()
        {
            var userId = CurrentUserService.UserId;
            return View(await Mediator.Send(new GetSoldFactorsQuery {UserId = userId}));
        }
    }
}