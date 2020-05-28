using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Tickets.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Tickets
{
    public class GetAllTicketsQuery : IRequest<List<TicketListVm>>
    {
    }

    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<TicketListVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllTicketsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TicketListVm>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Tickets.OrderByDescending(f => f.Id)
                .ProjectTo<TicketListVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}