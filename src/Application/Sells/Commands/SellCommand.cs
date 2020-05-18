using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Sells.Commands
{
    public class SellCommand : IRequest<int>
    {
        public double Amount { get; set; }
        public string CurrencyUrl { get; set; }
        public string TransactionLink { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }

    public class SellCommandHandler : IRequestHandler<SellCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public SellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(SellCommand request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(f => f.DisplayUrl == request.CurrencyUrl,
                cancellationToken);
            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.CurrencyUrl);

            var sell = new Sell
            {
                Amount = request.Amount,
                CurrencyId = currency.Id,
                TransactionLink = request.TransactionLink,
                Description = request.Description,
                UserId = request.UserId
            };

            _context.Sells.Add(sell);
            await _context.SaveChangesAsync(cancellationToken);

            return sell.Id;
        }
    }
}