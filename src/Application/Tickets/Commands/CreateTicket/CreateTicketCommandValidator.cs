using FluentValidation;

namespace Crypto.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketCommandValidator()
        {
            RuleFor(f => f.Topic)
                .NotEmpty().WithMessage("وارد کردن عنوان اجباری است")
                .MaximumLength(120).WithMessage("عنوان درخواست شما نباید بیشتر از 120 کاراکتر باشد");
            RuleFor(f => f.Description)
                .NotEmpty().WithMessage("وارد کردن توضیحات اجباری است");
        }
    }
}