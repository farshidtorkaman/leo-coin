using Crypto.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private bool _init;
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