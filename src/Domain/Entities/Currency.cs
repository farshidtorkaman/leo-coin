using System.Collections.Generic;
using Crypto.Domain.Common;

namespace Crypto.Domain.Entities
{
    public class Currency : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DisplayUrl { get; set; }
        public bool CanBePurchase { get; set; }
        public bool CanBeSell { get; set; }
        public string WalletId { get; set; }
        public virtual List<Purchase> Purchases { get; set; }
    }
}
