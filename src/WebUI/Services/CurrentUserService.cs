using Crypto.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // var httpContext = httpContextAccessor.HttpContext;
            // var user = httpContext?.User;
            // UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private bool _init = false;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;

        public string UserId
        {
            get
            {
                if (!_init)
                {
                    _userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                    _init = true;
                }

                return _userId;
            }
        }
    }
}
