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
    public class ConfirmSellCommand : IRequest
    {
        public int SellId { get; set; }
        public string TransactionCode { get; set; }
    }

    public class ConfirmSellCommandHandler : IRequestHandler<ConfirmSellCommand>
    {
        private readonly IApplicationDbContext _context;

        public ConfirmSellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConfirmSellCommand request, CancellationToken cancellationToken)
        {
            var sell =
                await _context.Sells.FirstOrDefaultAsync(f => f.Id == request.SellId, cancellationToken);
            if (sell == null)
                throw new NotFoundException(nameof(Purchase), request.SellId);

            sell.TrackingCode = request.TransactionCode;
            sell.Status = PurchaseStatus.Done;
            
            var currency =
                await _context.Currencies.FirstOrDefaultAsync(f => f.Id == sell.CurrencyId, cancellationToken);
            currency.Stock += sell.Amount;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}