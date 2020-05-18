using FluentValidation;

namespace Crypto.Application.Purchases.Commands
{
    public class PurchaseCommandValidator : AbstractValidator<PurchaseCommand>
    {
        public PurchaseCommandValidator()
        {
            RuleFor(f => f.Amount).NotEmpty().WithMessage("وارد کردن مقدار اجباری است");
            RuleFor(f => f.WalletId).NotEmpty().WithMessage("وارد آدرس کیف پول اجباری است");
        }
    }
}