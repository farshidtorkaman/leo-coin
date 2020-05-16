using System.Diagnostics;
using System.Security.Claims;
using Crypto.Application.Common.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private string userId;
        public HomeController(ILogger<HomeController> logger, ICurrentUserService currentUserService, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            userId = accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            if (User.IsAuthenticated())
            {
                var user = userId;
                var uuu = _currentUserService.UserId;

            }
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
