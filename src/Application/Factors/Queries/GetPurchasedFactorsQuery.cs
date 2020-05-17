using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Factors.Queries
{
    public class GetPurchasedFactorsQuery : IRequest<List<PurchasedFactorVm>>
    {
        public string UserId { get; set; }
    }

    public class GetPurchasedFactorsQueryHandler : IRequestHandler<GetPurchasedFactorsQuery, List<PurchasedFactorVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPurchasedFactorsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PurchasedFactorVm>> Handle(GetPurchasedFactorsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Purchases.Where(f => f.UserId == request.UserId).OrderByDescending(f => f.Created)
                .ProjectTo<PurchasedFactorVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}