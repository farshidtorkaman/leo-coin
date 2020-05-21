using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;

namespace Crypto.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommand : IRequest
    {
        public string Topic { get; set; }
        public string Description { get; set; }
    }

    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateTicketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket {Topic = request.Topic, Status = TicketStatus.Open};

            var ticketMessage = new TicketMessage {Text = request.Description};
            ticket.Messages.Add(ticketMessage);

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}