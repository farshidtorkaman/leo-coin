using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Currencies.Queries;
using Crypto.Application.Purchases.Commands;
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
    public class PurchaseController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager; 

        public PurchaseController(IPaymentService paymentService, UserManager<ApplicationUser> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }

        [Route("buy/{displayUrl}")]
        public async Task<IActionResult> Index(string displayUrl)
        {
            if (displayUrl == null)
                return BadRequest();

            if (!await Mediator.Send(new GetCurrencyQuery {DisplayUrl = displayUrl}))
                return NotFound("Currency Not Found!");

            return View();
        }

        [Route("buy/{displayUrl}")]
        [HttpPost]
        public async Task<IActionResult> Index(PurchaseCommand command, string displayUrl)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                command.CurrencyUrl = displayUrl;
                command.UserId = user.Id;
                var purchaseId = await Mediator.Send(command);

                var callBackUrl = Url.Action("Verify", "Purchase", new {purchaseId}, Request.Scheme);
                var redirect = await _paymentService.Pay(1000, callBackUrl);

                return Redirect(redirect);
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

        [Route("buy/verify")]
        public async Task<IActionResult> Verify(string status, string token, int purchaseId)
        {
            try
            {
                var (result, transactionId) = await _paymentService.VerifyAsync(status, token);
                if (result)
                {
                    var command = new FinalizePurchaseCommand
                    {
                        PurchaseId = purchaseId, TransactionId = transactionId
                    };

                    await Mediator.Send(command);

                    return RedirectToAction("Purchases", "Factors");
                }

                ViewData["ErrorMessage"] = "خطایی رخ داد";
                return View();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}