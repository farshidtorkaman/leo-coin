using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Sells.Commands;
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
    public class SellsController : ControllerBase
    {
        [Route("sell/{displayUrl}")]
        public async Task<IActionResult> Index(string displayUrl)
        {
            try
            {
                if (displayUrl == null)
                    return BadRequest();

                ViewData["WalletId"] = await Mediator.Send(new GetCurrencyWalletIdQuery {DisplayUrl = displayUrl});
                return View();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [Route("sell/{displayUrl}")]
        [HttpPost]
        [Authorize(Policy = "FullConfirmation")]
        public async Task<IActionResult> Index(SellCommand command, string displayUrl)
        {
            try
            {
                command.UserId = CurrentUserService.UserId;
                command.CurrencyUrl = displayUrl;

                await Mediator.Send(command);

                return RedirectToAction("Sells", "Factors");
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    ModelState.AddModelError(key, value[0]);
                }
                return View(command);
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}