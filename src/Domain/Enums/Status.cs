using System.ComponentModel.DataAnnotations;

namespace Crypto.Domain.Enums
{
    public enum Status
    {
        [Display(Name = "ارسال نشده")]
        NotSent,
        [Display(Name = "ارسال شده")]
        Sent,
        [Display(Name = "تایید شده")]
        Confirmed,
        [Display(Name = "تایید نشده")]
        Rejected
    }
}