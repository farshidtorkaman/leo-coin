using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto.Application.Admin.Dashboard.Command;
using Crypto.Application.Admin.Dashboard.Queries;
using Crypto.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class DashboardController : ControllerBase
    {
        [Route("dashboard")]
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                RecentPurchases = await Mediator.Send(new GetRecentPurchasesQuery()),
                RecentSells = await Mediator.Send(new GetRecentSellsQuery())
            };
            return View(model);
        }

        [Route("confirm_purchase")]
        public IActionResult ConfirmPurchase(int purchaseId, string fullName, string amount)
        {
            ViewData["PurchaseId"] = purchaseId;
            ViewData["FullName"] = fullName;
            ViewData["Amount"] = amount;
            return PartialView();
        }

        [Route("confirm_purchase")]
        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase(int purchaseId, string transactionLink)
        {
            try
            {
                var command = new ConfirmPurchaseCommand {PurchaseId = purchaseId, TransactionLink = transactionLink};
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [Route("reject_purchase")]
        public IActionResult RejectPurchase(int purchaseId, string fullName, string amount)
        {
            ViewData["PurchaseId"] = purchaseId;
            ViewData["FullName"] = fullName;
            ViewData["Amount"] = amount;
            return PartialView();
        }

        [Route("reject_purchase")]
        [HttpPost]
        public async Task<IActionResult> RejectPurchase(int purchaseId, string rejectReason)
        {
            try
            {
                var command = new RejectPurchaseCommand {PurchaseId = purchaseId, RejectReason = rejectReason};
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [Route("confirm_sell")]
        public IActionResult ConfirmSell(int sellId, string fullName, string amount)
        {
            ViewData["SellId"] = sellId;
            ViewData["FullName"] = fullName;
            ViewData["Amount"] = amount;
            return PartialView();
        }

        [Route("confirm_sell")]
        [HttpPost]
        public async Task<IActionResult> ConfirmSell(int sellId, string transactionCode)
        {
            try
            {
                var command = new ConfirmSellCommand {SellId = sellId, TransactionCode = transactionCode};
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [Route("reject_sell")]
        public IActionResult RejectSell(int sellId, string fullName, string amount)
        {
            ViewData["SellId"] = sellId;
            ViewData["FullName"] = fullName;
            ViewData["Amount"] = amount;
            return PartialView();
        }

        [Route("reject_sell")]
        [HttpPost]
        public async Task<IActionResult> RejectSell(int sellId, string rejectReason)
        {
            try
            {
                var command = new RejectSellCommand {SellId = sellId, RejectReason = rejectReason};
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }

    public class DashboardViewModel
    {
        public List<PurchaseVm> RecentPurchases { get; set; }
        public List<SellVm> RecentSells { get; set; }
    }
}