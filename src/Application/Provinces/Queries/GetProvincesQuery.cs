using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Provinces.Queries
{
    public class GetProvincesQuery : IRequest<IQueryable<Province>>
    {
    }

    public class GetProvincesQueryHandler : IRequestHandler<GetProvincesQuery, IQueryable<Province>>
    {
        private readonly IApplicationDbContext _context;

        public GetProvincesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Province>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
        {
            return _context.Provinces;
        }
    }
}