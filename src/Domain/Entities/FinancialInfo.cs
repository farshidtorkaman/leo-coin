using Crypto.Domain.Common;

namespace Crypto.Domain.Entities
{
    public class FinancialInfo : AuditableEntity
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
        public string UserId { get; set; }
    }
}