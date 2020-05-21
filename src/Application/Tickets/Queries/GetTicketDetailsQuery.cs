﻿using System.Linq;
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
        private readonly IMapper _mapper;

        public GetTicketDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TicketDetailsVm> Handle(GetTicketDetailsQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.Where(f => f.Id == request.Id)
                .ProjectTo<TicketDetailsVm>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
            if (ticket == null)
                throw new NotFoundException(nameof(Ticket), request.Id);

            return ticket;
        }
    }
}