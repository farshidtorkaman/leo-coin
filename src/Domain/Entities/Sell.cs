using Crypto.Domain.Common;
using Crypto.Domain.Enums;

namespace Crypto.Domain.Entities
{
    public class Sell : AuditableEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string UserId { get; set; }
        public PurchaseStatus Status { get; set; }
        public string TransactionLink { get; set; }
        public string RejectReason { get; set; }
        public string Description { get; set; }
        public string TrackingCode { get; set; }
    }
}