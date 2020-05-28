using System.Linq;
using Crypto.Application.Common.Interfaces;
using FluentValidation;

namespace Crypto.Application.Purchases.Commands
{
    public class PurchaseCommandValidator : AbstractValidator<PurchaseCommand>
    {
        private readonly IApplicationDbContext _context;
        public PurchaseCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(f => f.Amount).NotEmpty().WithMessage("وارد کردن مقدار اجباری است");
            RuleFor(f => f.WalletId).NotEmpty().WithMessage("وارد آدرس کیف پول اجباری است");
            RuleFor(f => f.Amount).Must(BeLessThanStock).WithMessage("موجودی وبسایت کافی نیست");
        }

        private bool BeLessThanStock(PurchaseCommand command, double amount)
        {
            var currency = _context.Currencies.FirstOrDefault(f => f.DisplayUrl == command.CurrencyUrl); 
            return amount <= currency?.Stock;
        }
    }
}