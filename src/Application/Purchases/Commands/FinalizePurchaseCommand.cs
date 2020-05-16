using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Purchases.Commands
{
    public class FinalizePurchaseCommand : IRequest
    {
        public int PurchaseId { get; set; }
        public string TransactionId { get; set; }
    }

    public class FinalizePurchaseCommandHandler : IRequestHandler<FinalizePurchaseCommand>
    {
        private readonly IApplicationDbContext _context;

        public FinalizePurchaseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(FinalizePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = await _context.Purchases.FindAsync(request.PurchaseId);
            if (purchase == null)
                throw new NotFoundException(nameof(Purchase), request.PurchaseId);

            purchase.IsDone = true;
            purchase.TransactionId = request.TransactionId;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}