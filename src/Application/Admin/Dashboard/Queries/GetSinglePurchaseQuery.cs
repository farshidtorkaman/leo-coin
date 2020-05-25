using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Dashboard.Queries
{
    public class GetSinglePurchaseQuery : IRequest<PurchaseVm>
    {
        public int PurchaseId { get; set; }
    }

    public class GetSinglePurchaseQueryHandler : IRequestHandler<GetSinglePurchaseQuery, PurchaseVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetSinglePurchaseQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<PurchaseVm> Handle(GetSinglePurchaseQuery request, CancellationToken cancellationToken)
        {
            var purchase = await _context.Purchases.Include(f => f.Currency)
                .FirstOrDefaultAsync(f => f.Id == request.PurchaseId, cancellationToken);
            if (purchase == null)
                throw new NotFoundException(nameof(Purchase), request.PurchaseId);

            return new PurchaseVm
            {
                Amount = purchase.Amount.ToString(CultureInfo.InvariantCulture) + " " + purchase.Currency.Title,
                PricePaid = purchase.PricePaid,
                PurchaseId = purchase.Id,
                TrackingCode = purchase.TransactionId,
                WalletId = purchase.WalletId,
                UserFullName = _identityService.GetFullName(purchase.UserId)
            };
        }
    }
}