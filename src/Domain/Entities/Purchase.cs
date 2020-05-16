namespace Crypto.Domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string WalletId { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public double PricePaid { get; set; }
        public bool IsDone { get; set; }
        public string TransactionId { get; set; }
    }
}