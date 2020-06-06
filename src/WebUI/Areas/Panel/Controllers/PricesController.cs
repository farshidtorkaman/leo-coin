using System.Threading.Tasks;
using Crypto.Application.PriceCalculations;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel")]
    public class PricesController : ControllerBase
    {
        [Route("price/{displayUrl}")]
        public async Task<JsonResult> Get(double amount, string displayUrl)
        {
            return Json(await Mediator.Send(new GetPriceCommand {Amount = amount, DisplayUrl = displayUrl}));
        }
    }
}