using Crypto.Domain.Enums;

namespace Crypto.Application.Admin.Dashboard.Queries
{
    public class PurchaseVm
    {
        public int PurchaseId { get; set; }
        public string UserFullName { get; set; }
        public string Amount { get; set; }
        public string WalletId { get; set; }
        public double PricePaid { get; set; }
        public string TrackingCode { get; set; }
        public PurchaseStatus Status { get; set; }
    }
}