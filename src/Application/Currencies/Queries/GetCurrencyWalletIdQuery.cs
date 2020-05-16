using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Currencies.Queries
{
    public class GetCurrencyWalletIdQuery : IRequest<string>
    {
        public string DisplayUrl { get; set; }
    }

    public class GetCurrencyWalletIdQueryHandler : IRequestHandler<GetCurrencyWalletIdQuery, string>
    {
        private readonly IApplicationDbContext _context;

        public GetCurrencyWalletIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetCurrencyWalletIdQuery request, CancellationToken cancellationToken)
        {
            var walletId = await _context.Currencies.Where(f => f.DisplayUrl == request.DisplayUrl)
                .Select(f => f.WalletId)
                .SingleOrDefaultAsync(cancellationToken);
            if (walletId == null)
                throw new NotFoundException(nameof(Currency), request.DisplayUrl);
            
            return walletId;
        }
    }
}