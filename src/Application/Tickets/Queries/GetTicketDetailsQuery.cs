using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Tickets.Queries
{
    public class GetTicketDetailsQuery : IRequest<TicketDetailsVm>
    {
        public int Id { get; set; }
    }

    public class GetTicketDetailsQueryHandler : IRequestHandler<GetTicketDetailsQuery, TicketDetailsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetTicketDetailsQueryHandler(IApplicationDbContext context, IMapper mapper,
            ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TicketDetailsVm> Handle(GetTicketDetailsQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.Where(f => f.Id == request.Id)
                .ProjectTo<TicketDetailsVm>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);

            var currentUserId = _currentUserService.UserId;
            var isAdmin = await _identityService.IsInRoleAsync(currentUserId, "admin");
            if (ticket == null || ticket.CreatedBy != currentUserId && !isAdmin)
                throw new NotFoundException(nameof(Ticket), request.Id);

            return ticket;
        }
    }
}