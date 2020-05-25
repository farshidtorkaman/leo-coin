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
    public class GetRecentPurchasesQuery : IRequest<List<PurchaseVm>>
    {
    }

    public class GetRecentPurchasesQueryHandler : IRequestHandler<GetRecentPurchasesQuery, List<PurchaseVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetRecentPurchasesQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<List<PurchaseVm>> Handle(GetRecentPurchasesQuery request,
            CancellationToken cancellationToken)
        {
            var purchases = await _context.Purchases.Include(f => f.Currency).OrderByDescending(f => f.Id).Take(6)
                .ToListAsync(cancellationToken);
            if (purchases.Any())
            {
                return purchases.Select(purchase => new PurchaseVm
                    {
                        Amount = purchase.Amount.ToString(CultureInfo.InvariantCulture) + " " + purchase.Currency.Title,
                        PricePaid = purchase.PricePaid,
                        PurchaseId = purchase.Id,
                        TrackingCode = purchase.TransactionId,
                        WalletId = purchase.WalletId,
                        UserFullName = _identityService.GetFullName(purchase.UserId),
                        Status = purchase.Status
                    })
                    .ToList();
            }

            return new List<PurchaseVm>();
        }
    }
}