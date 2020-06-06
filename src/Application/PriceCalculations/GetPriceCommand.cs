using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.PriceCalculations
{
    public class GetPriceCommand : IRequest<string>
    {
        public double Amount { get; set; }
        public string DisplayUrl { get; set; }
    }
    
    public class GetPriceCommandHandler : IRequestHandler<GetPriceCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICryptoService _cryptoService;

        public GetPriceCommandHandler(IApplicationDbContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<string> Handle(GetPriceCommand request, CancellationToken cancellationToken)
        {
            var currency =
                await _context.Currencies.SingleOrDefaultAsync(f => f.DisplayUrl == request.DisplayUrl,
                    cancellationToken);
            if(currency == null)
                throw new NotFoundException(nameof(Currency), request.DisplayUrl);

            return $"{await _cryptoService.ConvertToToman(request.Amount, currency.Symbol):n0}";
        }
    }
}