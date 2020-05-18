using System.Threading.Tasks;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public FactorsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("buys")]
        public async Task<IActionResult> Purchases()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return View(await Mediator.Send(new GetPurchasedFactorsQuery {UserId = user.Id}));
        }

        [Route("sells")]
        public async Task<IActionResult> Sells()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await Mediator.Send(new GetSoldFactorsQuery {UserId = user.Id}));
        }
    }
}