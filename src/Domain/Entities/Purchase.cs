using Crypto.Domain.Common;
using Crypto.Domain.Enums;

namespace Crypto.Domain.Entities
{
    public class Purchase : AuditableEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string WalletId { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public double PricePaid { get; set; }
        public bool IsDone { get; set; }
        public string TransactionId { get; set; }
        public string UserId { get; set; }
        public PurchaseStatus Status { get; set; }
        public string TransactionLink { get; set; }
        public string RejectReason { get; set; }
    }
}