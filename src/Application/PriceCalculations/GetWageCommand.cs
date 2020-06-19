using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.PriceCalculations
{
    public class GetWageCommand : IRequest<double>
    {
        public double Amount { get; set; }
        public string DisplayUrl { get; set; }
    }

    public class GetWageCommandHandler : IRequestHandler<GetWageCommand, double>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICryptoService _cryptoService;

        public GetWageCommandHandler(IApplicationDbContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<double> Handle(GetWageCommand request, CancellationToken cancellationToken)
        {
            var currency =
                await _context.Currencies.SingleOrDefaultAsync(f => f.DisplayUrl == request.DisplayUrl,
                    cancellationToken);
            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.DisplayUrl);

            return _cryptoService.GetWage(request.Amount, currency.Symbol) / 10;
        }
    }
}
