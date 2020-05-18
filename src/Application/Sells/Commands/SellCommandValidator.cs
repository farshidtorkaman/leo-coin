using FluentValidation;

namespace Crypto.Application.Sells.Commands
{
    public class SellCommandValidator : AbstractValidator<SellCommand>
    {
        public SellCommandValidator()
        {
            RuleFor(f => f.Amount).NotEmpty().WithMessage("وارد کردن مقدار اجباری است");
            RuleFor(f => f.TransactionLink).NotEmpty().WithMessage("وارد کردن لینک تراکنش اجباری است");    
        }
    }
}