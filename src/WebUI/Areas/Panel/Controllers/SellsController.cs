﻿using System.Threading.Tasks;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Sells.Commands;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel")]
    public class SellsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SellsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("sell/{displayUrl}")]
        public async Task<IActionResult> Index(string displayUrl)
        {
            if (displayUrl == null)
                return BadRequest();

            if (!await Mediator.Send(new GetCurrencyQuery {DisplayUrl = displayUrl}))
                return NotFound();
            
            return View();
        }

        [Route("sell/{displayUrl}")]
        [HttpPost]
        public async Task<IActionResult> Index(SellCommand command, string displayUrl)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            command.UserId = user.Id;
            command.CurrencyUrl = displayUrl;
            
            await Mediator.Send(command);

            return RedirectToAction("Sells", "Factors");
        }
    }
}