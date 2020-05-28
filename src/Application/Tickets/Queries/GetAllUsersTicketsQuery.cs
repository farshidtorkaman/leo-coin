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
    public class GetAllUsersTicketsQuery : IRequest<List<TicketListVm>>
    {
    }

    public class GetAllUsersTicketsQueryHandler : IRequestHandler<GetAllUsersTicketsQuery, List<TicketListVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllUsersTicketsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<List<TicketListVm>> Handle(GetAllUsersTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.Tickets.Where(f => f.CreatedBy == userId).OrderByDescending(f => f.Id)
                .ProjectTo<TicketListVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}