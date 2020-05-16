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
    public class GetCurrencyQuery : IRequest<bool>
    {
        public string DisplayUrl { get; set; }
    }

    public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public GetCurrencyQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
        {
            return await _context.Currencies.AnyAsync(f => f.DisplayUrl == request.DisplayUrl,
                cancellationToken);
        }
    }
}