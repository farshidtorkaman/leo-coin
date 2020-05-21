using System.Linq;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Tickets.Commands.CreateTicket;
using Crypto.Application.Tickets.Commands.ReplyTicket;
using Crypto.Application.Tickets.Queries;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public TicketsController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [Route("list")]
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new GetAllTicketsQuery()));
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    foreach (var singleValue in value)
                    {
                        ModelState.AddModelError(key, singleValue);
                    }
                }

                return View(command);
            }
        }

        [Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                var model = await Mediator.Send(new GetTicketDetailsQuery {Id = (int) id});
                
                var lastSubmitter = model.Messages.OrderByDescending(f => f.Id).Select(f => f.CreatedBy).First();
                ViewBag.UserCanReply = lastSubmitter != _currentUserService.UserId;
                
                return View(model);
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [Route("reply")]
        [HttpPost]
        public async Task<IActionResult> Reply(ReplyTicketCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    foreach (var singleValue in value)
                    {
                        ModelState.AddModelError(key, singleValue);
                    }
                }
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            
            return RedirectToAction("Details", new {id = command.TicketId});
        }
    }
}