using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [Display(Name = "کلمه عبور فعلی")]
        public string OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "وارد کردن {0} اجباری است")]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "با رمز عبور تطابق ندارد")]
        [Display(Name = "تایید کلمه عبور")]
        public string ConfirmNewPassword { get; set; }
    }
}