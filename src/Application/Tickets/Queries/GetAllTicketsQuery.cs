using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Tickets.Queries
{
    public class GetAllTicketsQuery : IRequest<List<TicketListVm>>
    {
        
    }
    
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<TicketListVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllTicketsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<List<TicketListVm>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.Tickets.Where(f => f.CreatedBy == userId)
                .ProjectTo<TicketListVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}