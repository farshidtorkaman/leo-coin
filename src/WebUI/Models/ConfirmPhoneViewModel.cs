using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ConfirmPhoneViewModel
    {
        public string PhoneNumber { get; set; }
     
        [Required(ErrorMessage = "کد تاییدیه را وارد نمایید")]
        [MaxLength(6, ErrorMessage = "کد تاییدیه باید 6 رقم باشد")]
        public string Token { get; set; }
    }
}