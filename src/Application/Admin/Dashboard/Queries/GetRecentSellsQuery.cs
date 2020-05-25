using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Dashboard.Queries
{
    public class GetRecentSellsQuery : IRequest<List<SellVm>>
    {
    }

    public class GetRecentSellsQueryHandler : IRequestHandler<GetRecentSellsQuery, List<SellVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetRecentSellsQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<List<SellVm>> Handle(GetRecentSellsQuery request, CancellationToken cancellationToken)
        {
            var sells = await _context.Sells.Include(f => f.Currency).OrderByDescending(f => f.Id).Take(6)
                .ToListAsync(cancellationToken);
            if (!sells.Any()) return new List<SellVm>();

            return sells.Select(sell => new SellVm
                {
                    Amount = sell.Amount.ToString(CultureInfo.InvariantCulture) + " " + sell.Currency.Title,
                    Sheba = _identityService.GetSheba(sell.UserId),
                    SellId = sell.Id,
                    TransactionLink = sell.TransactionLink,
                    UserFullName = _identityService.GetFullName(sell.UserId),
                    Status = sell.Status
                })
                .ToList();
        }
    }
}