using FluentValidation;

namespace Crypto.Application.Users.FinancialInformation.Commands
{
    public class UpdateFinancialInfoCommandValidator : AbstractValidator<UpdateFinancialInfoCommand>
    {
        public UpdateFinancialInfoCommandValidator()
        {
            RuleFor(f => f.CardNumber)
                .Length(16).WithMessage("شماره کارت معتبر وارد نمایید");
            RuleFor(f => f.AccountOwnerName)
                .MaximumLength(150).WithMessage("بیشتر از حد مجاز کاراکتر وارد کردید");
            RuleFor(f => f.Sheba)
                .Length(24).WithMessage("شماره شبای معتبر وارد نمایید");

        }
    }
}