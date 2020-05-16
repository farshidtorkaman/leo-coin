using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;

namespace Crypto.Application.Currencies.Queries
{
    public class CurrencyDashboardVm : IMapFrom<Currency>
    {
        public string Title { get; set; }
        public string DisplayUrl { get; set; }
    }
}