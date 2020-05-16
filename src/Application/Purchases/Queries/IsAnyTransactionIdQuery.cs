using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Purchases.Queries
{
    public class IsAnyTransactionIdQuery : IRequest<bool>
    {
        public string TransactionId { get; set; }
    }

    public class IsAnyTransactionIdQueryHandler : IRequestHandler<IsAnyTransactionIdQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public IsAnyTransactionIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(IsAnyTransactionIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Purchases.AnyAsync(f => f.TransactionId == request.TransactionId, cancellationToken);
        }
    }
}