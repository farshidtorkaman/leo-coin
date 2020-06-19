using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.PriceCalculations;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Purchases.Commands
{
    public class PurchaseCommand : IRequest<(int, double)>
    {
        public double Amount { get; set; }
        public string WalletId { get; set; }
        public string CurrencyUrl { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }

    public class PurchaseCommandHandler : IRequestHandler<PurchaseCommand, (int, double)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public PurchaseCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<(int, double)> Handle(PurchaseCommand request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(f => f.DisplayUrl == request.CurrencyUrl,
                cancellationToken);
            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.CurrencyUrl);

            var tomanPrice =
                await _mediator.Send(new GetPriceCommand { Amount = request.Amount, DisplayUrl = request.CurrencyUrl },
                    cancellationToken);
            var wage = await _mediator.Send(
                new GetWageCommand { Amount = request.Amount, DisplayUrl = request.CurrencyUrl }, cancellationToken);

            var purchase = new Purchase
            {
                Amount = request.Amount,
                CurrencyId = currency.Id,
                WalletId = request.WalletId,
                Description = request.Description,
                UserId = request.UserId,
                PricePaid = tomanPrice + wage
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync(cancellationToken);

            return (purchase.Id, purchase.PricePaid);
        }
    }
}