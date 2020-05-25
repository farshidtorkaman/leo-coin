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
    public class ConfirmPurchaseCommand : IRequest
    {
        public int PurchaseId { get; set; }
        public string TransactionLink { get; set; }
    }

    public class ConfirmPurchaseCommandHandler : IRequestHandler<ConfirmPurchaseCommand>
    {
        private readonly IApplicationDbContext _context;

        public ConfirmPurchaseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConfirmPurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase =
                await _context.Purchases.FirstOrDefaultAsync(f => f.Id == request.PurchaseId, cancellationToken);
            if (purchase == null)
                throw new NotFoundException(nameof(Purchase), request.PurchaseId);

            purchase.TransactionLink = request.TransactionLink;
            purchase.Status = PurchaseStatus.Done;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}