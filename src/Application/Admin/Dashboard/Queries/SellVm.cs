using Crypto.Domain.Enums;

namespace Crypto.Application.Admin.Dashboard.Queries
{
    public class SellVm
    {
        public int SellId { get; set; }
        public string UserFullName { get; set; }
        public string Amount { get; set; }
        public string TransactionLink { get; set; }
        public string Sheba { get; set; }
        public PurchaseStatus Status { get; set; }
    }
}