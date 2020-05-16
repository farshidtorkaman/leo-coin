using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Currencies.Commands
{
    public class AddCurrencyCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string DisplayUrl { get; set; }
        public bool CanBePurchased { get; set; }
        public bool CanBeSell { get; set; }
    }

    public class AddCurrencyCommandHandler : IRequestHandler<AddCurrencyCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddCurrencyCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = new Currency
            {
                Title = request.Title,
                DisplayUrl = request.DisplayUrl,
                CanBePurchase = request.CanBePurchased,
                CanBeSell = request.CanBeSell
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync(cancellationToken);
            return currency.Id;
        }
    }
}