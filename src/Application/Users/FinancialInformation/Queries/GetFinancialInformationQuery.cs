using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.FinancialInformation.Queries
{
    public class GetFinancialInformationQuery : IRequest<List<FinancialInfoVm>>
    {
    }

    public class GetFinancialInformationQueryHandler : IRequestHandler<GetFinancialInformationQuery, List<FinancialInfoVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetFinancialInformationQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<FinancialInfoVm>> Handle(GetFinancialInformationQuery request, CancellationToken cancellationToken)
        {
            return await _context.FinancialInformation.Where(f => f.CreatedBy == _currentUserService.UserId)
                .ProjectTo<FinancialInfoVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}