using System.Threading.Tasks;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string role)
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
            return RedirectToAction("Create");
        }
    }
}