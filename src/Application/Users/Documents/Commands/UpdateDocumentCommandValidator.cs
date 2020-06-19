using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Crypto.Application.Users.Documents.Commands
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        private readonly string[] _permittedExtensions = {".jpeg", ".jpg", ".png"};

        public UpdateDocumentCommandValidator()
        {
            RuleFor(f => f.NationalCode)
                .NotEmpty().WithMessage("کد ملی خود را وارد نمایید")
                .Must(BeIranianNationalCode).WithMessage("کد ملی معتبر وارد نمایید");
            RuleFor(f => f.BirthDate)
                .NotEmpty().WithMessage("تاریخ تولد خود را وارد نمایید");
            RuleFor(f => f.NationalCardImage)
                .Must(BeImage).WithMessage("فرمت فایل ارسالی پشتیبانی نمیشود")
                .Must(BeMaximumFiveMeg).WithMessage("حداکثر میتوانید پنج مگابایت عکس آپلود نمایید");
            RuleFor(f => f.ApplicantImage)
                .Must(BeImage).WithMessage("فرمت فایل ارسالی پشتیبانی نمیشود")
                .Must(BeMaximumFiveMeg).WithMessage("حداکثر میتوانید پنج مگابایت عکس آپلود نمایید");
        }

        private static bool BeIranianNationalCode(string nationalCode)
        {
            // if input is null ignore it
            if (string.IsNullOrEmpty(nationalCode))
                return true;
            
            if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
                return false;

            var check = Convert.ToInt32(nationalCode.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(nationalCode.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return sum < 2 && check == sum || sum >= 2 && check + sum == 11;
        }

        private static bool BeMaximumFiveMeg(UpdateDocumentCommand command, IFormFile file)
        {
            // if the file is null ignore it
            if (file == null)
                return true;

            // if file is greater that 5 megabyte it will return false
            return file.Length <= 5242880;
        }

        private bool BeImage(UpdateDocumentCommand command, IFormFile file)
        {
            // if the file is null ignore it
            if (file == null)
                return true;

            var extension = Path.GetExtension(file.FileName);
            return !string.IsNullOrEmpty(extension) && _permittedExtensions.Contains(extension);
        }
    }
}