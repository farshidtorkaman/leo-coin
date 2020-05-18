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
    public class GetSoldFactorsQuery : IRequest<List<SoldFactorVm>>
    {
        public string UserId { get; set; }
    }
    
    public class GetSoldFactorsQueryHandler : IRequestHandler<GetSoldFactorsQuery, List<SoldFactorVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSoldFactorsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SoldFactorVm>> Handle(GetSoldFactorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Sells.Where(f => f.UserId == request.UserId).OrderByDescending(f => f.Created)
                .ProjectTo<SoldFactorVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}