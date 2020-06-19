using FluentValidation;

namespace Crypto.Application.Users.FinancialInformation.Commands
{
    public class UpdateFinancialInfoCommandValidator : AbstractValidator<UpdateFinancialInfoCommand>
    {
        public UpdateFinancialInfoCommandValidator()
        {
            RuleFor(f => f.CardNumber)
                .NotEmpty().WithMessage("شماره کارت خود را وارد کنید")
                .Length(16).WithMessage("شماره کارت معتبر وارد نمایید");
            RuleFor(f => f.AccountNumber)
                .NotEmpty().WithMessage("شماره حساب خود را وارد کنید");
            RuleFor(f => f.BankId)
                .NotEmpty().WithMessage("بانک خود را انتخاب نمایید");
            RuleFor(f => f.Sheba)
                .NotEmpty().WithMessage("شماره شبای خود را وارد کنید")
                .Length(24).WithMessage("شماره شبای معتبر وارد نمایید");
            RuleFor(f => f.BankCardImage)
                .NotEmpty().WithMessage("عکس کارت بانکی خود را بارگذاری نمایید");
        }
    }
}