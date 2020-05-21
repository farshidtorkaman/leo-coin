using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Crypto.Domain.Enums
{
    public enum TicketStatus
    {
        [Display(Name = "باز")]
        Open,
        [Display(Name = "بسته")]
        Close
    }
}