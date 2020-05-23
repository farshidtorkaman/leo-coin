using System.Threading.Tasks;
using Crypto.Application.Admin.Users.Queries;
using Crypto.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("admin")]
    [Route("admin")]
    public class UsersController : ControllerBase
    {
        [Route("users/list")]
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new GetUsersQuery()));
        }

        [Route("users/details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                return View(await Mediator.Send(new GetUserDetailQuery {UserId = id}));
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}