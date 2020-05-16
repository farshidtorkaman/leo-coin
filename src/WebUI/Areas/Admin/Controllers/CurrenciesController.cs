using System;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Currencies.Commands;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/currencies")]
    public class CurrenciesController : ControllerBase
    {
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(AddCurrencyCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Create));
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    ModelState.AddModelError(key, value[0]);
                }
                return View(command);
            }
        }
    }
}