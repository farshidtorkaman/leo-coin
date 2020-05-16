using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.Profile.Commands
{
    public class ConfirmPhoneNumberCommand : IRequest
    {
        public string PhoneNumber { get; set; }
    }

    public class ConfirmPhoneNumberCommandHandler : IRequestHandler<ConfirmPhoneNumberCommand>
    {
        private readonly IApplicationDbContext _context;

        public ConfirmPhoneNumberCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConfirmPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var profile =
                await _context.UsersProfiles.SingleOrDefaultAsync(f => f.PhoneNumber == request.PhoneNumber,
                    cancellationToken);
            if (profile == null)
                throw new NotFoundException(nameof(UserProfile), request.PhoneNumber);

            profile.PhoneNumberConfirmed = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}