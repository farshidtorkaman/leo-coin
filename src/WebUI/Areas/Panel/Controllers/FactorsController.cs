using System.Threading.Tasks;
using Crypto.Application.Factors.Queries;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel")]
    public class FactorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FactorsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("factors")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return View(await Mediator.Send(new GetPurchasedFactorsQuery {UserId = user.Id}));
        }
    }
}