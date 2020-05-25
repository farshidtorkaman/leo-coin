using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Confirms
{
    public class ConfirmApplicantCommand : IRequest
    {
        public string UserId { get; set; }
        public bool IsConfirm { get; set; }
    }

    public class ConfirmApplicantCommandHandler : IRequestHandler<ConfirmApplicantCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ConfirmApplicantCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(ConfirmApplicantCommand request, CancellationToken cancellationToken)
        {
            var username = await _identityService.GetUserNameAsync(request.UserId);
            if (username == null)
                throw new NotFoundException();

            var document =
                await _context.Documents.FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);
            if (document == null)
                throw new NotFoundException(nameof(Document), request.UserId);

            document.ApplicantImageStatus =
                request.IsConfirm ? DocumentImagesStatus.Confirmed : DocumentImagesStatus.Rejected;

            await _context.SaveChangesAsync(cancellationToken);
            if(request.IsConfirm)
                await _identityService.AddConfirmsClaim(request.UserId, "Applicant");

            return Unit.Value;
        }
    }
}