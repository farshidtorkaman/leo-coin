using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;

namespace Crypto.Application.Tickets.Commands.ReplyTicket
{
    public class ReplyTicketCommand : IRequest
    {
        public int TicketId { get; set; }
        public string Text { get; set; }
        public bool Close { get; set; }
    }

    public class ReplyTicketCommandHandler : IRequestHandler<ReplyTicketCommand>
    {
        private readonly IApplicationDbContext _context;

        public ReplyTicketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReplyTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.FindAsync(request.TicketId);
            if (ticket == null)
                throw new NotFoundException(nameof(Ticket), request.TicketId);

            ticket.Messages.Add(new TicketMessage {Text = request.Text, TicketId = request.TicketId});
            if (request.Close)
                ticket.Status = TicketStatus.Close;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}