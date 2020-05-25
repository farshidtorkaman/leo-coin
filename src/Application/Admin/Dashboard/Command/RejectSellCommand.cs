using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Dashboard.Command
{
    public class RejectSellCommand : IRequest
    {
        public int SellId { get; set; }
        public string RejectReason { get; set; }
    }

    public class RejectSellCommandHandler : IRequestHandler<RejectSellCommand>
    {
        private readonly IApplicationDbContext _context;

        public RejectSellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RejectSellCommand request, CancellationToken cancellationToken)
        {
            var sell =
                await _context.Sells.FirstOrDefaultAsync(f => f.Id == request.SellId, cancellationToken);
            if (sell == null)
                throw new NotFoundException(nameof(Purchase), request.SellId);

            sell.RejectReason = request.RejectReason;
            sell.Status = PurchaseStatus.Rejected;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}