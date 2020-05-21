using System.Linq;
using Crypto.Application.Common.Interfaces;
using FluentValidation;
using FluentValidation.Validators;

namespace Crypto.Application.Tickets.Commands.ReplyTicket
{
    public class ReplyTextValidator : AbstractValidator<ReplyTicketCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ReplyTextValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(f => f.TicketId).Must(NotBeLastSubmitter).WithMessage("شما اجازه پاسخ ندارید");
        }

        private bool NotBeLastSubmitter(ReplyTicketCommand command, int ticketId)
        {
            var lastSubmitter = _context.TicketMessages.Where(f => f.TicketId == ticketId)
                .OrderByDescending(f => f.Id).Select(f => f.CreatedBy).First();
            
            return lastSubmitter != _currentUserService.UserId;
        }
    }
}