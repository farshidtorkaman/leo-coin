using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Purchases.Commands
{
    public class PurchaseCommand : IRequest<int>
    {
        public double Amount { get; set; }
        public string WalletId { get; set; }
        public string CurrencyUrl { get; set; }
    }

    public class PurchaseCommandHandler : IRequestHandler<PurchaseCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public PurchaseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(PurchaseCommand request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(f => f.DisplayUrl == request.CurrencyUrl,
                cancellationToken: cancellationToken);
            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.CurrencyUrl);

            var purchase = new Purchase
            {
                Amount = request.Amount,
                CurrencyId = currency.Id,
                WalletId = request.WalletId,
                PricePaid = 0
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync(cancellationToken);

            return purchase.Id;
        }
    }
}