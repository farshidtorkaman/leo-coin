using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private IMediator _mediator;
        private ICurrentUserService _currentUserService;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ICurrentUserService CurrentUserService =>
            _currentUserService ??= HttpContext.RequestServices.GetService<ICurrentUserService>();
    }
}