using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;

namespace Crypto.Application.Users.FinancialInformation.Queries
{
    public class FinancialInfoVm : IMapFrom<FinancialInfo>
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int? BankId { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
    }
}