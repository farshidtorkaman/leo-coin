using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Users.FinancialInformation.Queries
{
    public class FinancialInfoVm : IMapFrom<FinancialInfo>
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
        public Status Status { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<FinancialInfo, FinancialInfoVm>()
                .ForMember(dst => dst.BankName, opt => opt.MapFrom(src => src.Bank.Title));
        }
    }
}