using System;
using System.Threading.Tasks;
using Crypto.Application.Admin.Confirms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/confirm")]
    [Authorize(Roles = "admin")]
    public class ConfirmsController : ControllerBase
    {
        [Route("tell")]
        [HttpPost]
        public async Task<IActionResult> ConfirmTell(string userId, bool isConfirm)
        {
            try
            {
                var command = new ConfirmTellCommand {UserId = userId, IsConfirm = isConfirm};
                await Mediator.Send(command);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [Route("national_card")]
        [HttpPost]
        public async Task<IActionResult> ConfirmNationalCard(string userId, bool isConfirm)
        {
            try
            {
                await Mediator.Send(new ConfirmNationalCardCommand {UserId = userId, IsConfirm = isConfirm});
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [Route("bank_card")]
        [HttpPost]
        public async Task<IActionResult> ConfirmBankCard(string userId, bool isConfirm, int financialId)
        {
            try
            {
                await Mediator.Send(new ConfirmBankCardCommand {UserId = userId, IsConfirm = isConfirm, FinancialId = financialId});
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [Route("applicant")]
        [HttpPost]
        public async Task<IActionResult> ConfirmApplicant(string userId, bool isConfirm)
        {
            try
            {
                await Mediator.Send(new ConfirmApplicantCommand {UserId = userId, IsConfirm = isConfirm});
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}