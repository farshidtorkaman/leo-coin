using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.Profile.Commands
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProfileCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(f => f.FirstName)
                .NotEmpty().WithMessage("لطفا نام خود را وارد نمایید")
                .MaximumLength(50).WithMessage("حداکثر 50 کاراکتر میتوانید وارد کنید");
            RuleFor(f => f.LastName)
                .NotEmpty().WithMessage("لطفا نام خانوادگی خود را وارد نمایید")
                .MaximumLength(50).WithMessage("حداکثر 50 کاراکتر میتوانید وارد کنید");
            RuleFor(f => f.PhoneNumber)
                .Length(11).WithMessage("تلفن همراه معتبر وارد نمایید")
                .MustAsync(BeUniquePhoneNumber).WithMessage("تلفن همراه وارد شده تکراری است");
            RuleFor(f => f.Tell).Length(11).WithMessage("تلفن ثابت معتبر وارد نمایید");
            RuleFor(f => f.PostalCode).Length(10).WithMessage("کد پستی باید 10 رقم باشد");
        }

        public async Task<bool> BeUniquePhoneNumber(UpdateProfileCommand model, string phoneNumber,
            CancellationToken cancellationToken)
        {
            // if phone number is null ignore it
            if (phoneNumber == null)
                return true;
            
            return await _context.UsersProfiles.Where(f => f.Id != model.Id)
                .AllAsync(f => f.PhoneNumber != phoneNumber, cancellationToken);
        }
    }
}
