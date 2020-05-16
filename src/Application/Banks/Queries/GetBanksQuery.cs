using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Banks.Queries
{
    public class GetBanksQuery : IRequest<IQueryable<Bank>>
    {
    }

    public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, IQueryable<Bank>>
    {
        private readonly IApplicationDbContext _context;

        public GetBanksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Bank>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
        {
            return _context.Banks;
        }
    }
}