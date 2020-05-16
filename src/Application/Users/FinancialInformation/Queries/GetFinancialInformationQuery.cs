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
    public class GetFinancialInformationQuery : IRequest<FinancialInfoVm>
    {
        public string UserId { get; set; }
    }

    public class GetFinancialInformationQueryHandler : IRequestHandler<GetFinancialInformationQuery, FinancialInfoVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFinancialInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FinancialInfoVm> Handle(GetFinancialInformationQuery request, CancellationToken cancellationToken)
        {
            return await _context.FinancialInformation.Where(f => f.UserId == request.UserId)
                .ProjectTo<FinancialInfoVm>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        }
    }
}