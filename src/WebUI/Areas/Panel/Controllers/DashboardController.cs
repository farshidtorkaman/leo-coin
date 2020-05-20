using System.Security.Claims;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Reports.Queries;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("dashboard")]
        public async Task<IActionResult> Index()
        {
            var userId = _currentUserService.UserId;
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