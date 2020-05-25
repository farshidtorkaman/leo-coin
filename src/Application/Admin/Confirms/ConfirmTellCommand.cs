using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Confirms
{
    public class ConfirmTellCommand : IRequest
    {
        public string UserId { get; set; }
        public bool IsConfirm { get; set; }
    }
    
    public class ConfirmTellCommandHandler : IRequestHandler<ConfirmTellCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ConfirmTellCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(ConfirmTellCommand request, CancellationToken cancellationToken)
        {
            var username = await _identityService.GetUserNameAsync(request.UserId);
            if(username == null)
                throw new NotFoundException();

            var profile =
                await _context.UsersProfiles.FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);
            if(profile == null)
                throw new NotFoundException(nameof(UserProfile), request.UserId);

            profile.TellConfirmed = request.IsConfirm;
            
            await _context.SaveChangesAsync(cancellationToken);
            if(request.IsConfirm)
                await _identityService.AddConfirmsClaim(request.UserId, "Tell");
            
            return Unit.Value;
        }
    }
}