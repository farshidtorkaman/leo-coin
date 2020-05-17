using System;
using System.Globalization;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Factors.Queries
{
    public class PurchasedFactorVm : IMapFrom<Purchase>
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string WalletId { get; set; }
        public string TransactionId { get; set; }
        public DateTime DateCreated { get; set; }
        public PurchaseStatus Status { get; set; }
        public string TransactionLink { get; set; }
        public string RejectReason { get; set; }
        
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Purchase, PurchasedFactorVm>()
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount.ToString(CultureInfo.InvariantCulture) + " " + src.Currency.Title));
        }
    }
}