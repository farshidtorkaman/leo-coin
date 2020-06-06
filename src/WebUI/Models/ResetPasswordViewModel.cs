using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "عدم تطابق با رمز عبور")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
