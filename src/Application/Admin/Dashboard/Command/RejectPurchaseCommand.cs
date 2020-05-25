using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Dashboard.Command
{
    public class RejectPurchaseCommand : IRequest
    {
        public int PurchaseId { get; set; }
        public string RejectReason { get; set; }
    }

    public class RejectPurchaseCommandHandler : IRequestHandler<RejectPurchaseCommand>
    {
        private readonly IApplicationDbContext _context;

        public RejectPurchaseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RejectPurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase =
                await _context.Purchases.FirstOrDefaultAsync(f => f.Id == request.PurchaseId, cancellationToken);
            if (purchase == null)
                throw new NotFoundException(nameof(Purchase), request.PurchaseId);

            purchase.RejectReason = request.RejectReason;
            purchase.Status = PurchaseStatus.Rejected;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}