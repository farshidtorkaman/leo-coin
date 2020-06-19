using System.IO;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Crypto.Application.Users.FinancialInformation.Commands
{
    public class CreateFinancialInfoCommandValidator : AbstractValidator<CreateFinancialInfoCommand>
    {
        private readonly string[] _permittedExtensions = {".jpeg", ".jpg", ".png"};
        
        public CreateFinancialInfoCommandValidator()
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
                .NotEmpty().WithMessage("عکس کارت بانکی خود را بارگذاری نمایید")
                .Must(BeImage).WithMessage("فرمت عکس ارسالی پشتیبانی نمیشود")
                .Must(BeMaximumFiveMeg).WithMessage("عکس آپلود شده نباید بیشتر از 5 مگابایت میباشد");
        }
        
        private static bool BeMaximumFiveMeg(CreateFinancialInfoCommand command, IFormFile file)
        {
            // if the file is null ignore it
            if (file == null)
                return true;

            // if file is greater that 5 megabyte it will return false
            return file.Length <= 5242880;
        }
        
        private bool BeImage(CreateFinancialInfoCommand command, IFormFile file)
        {
            // if the file is null ignore it
            if (file == null)
                return true;

            var extension = Path.GetExtension(file.FileName);
            return !string.IsNullOrEmpty(extension) && _permittedExtensions.Contains(extension);
        }
    }
}