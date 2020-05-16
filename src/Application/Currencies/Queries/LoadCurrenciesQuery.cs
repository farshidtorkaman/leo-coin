using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Currencies.Queries
{
    public class LoadCurrenciesQuery : IRequest<List<CurrencyDashboardVm>>
    {
    }

    public class LoadCurrenciesQueryHandler : IRequestHandler<LoadCurrenciesQuery, List<CurrencyDashboardVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public LoadCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDashboardVm>> Handle(LoadCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Currencies.ProjectTo<CurrencyDashboardVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}