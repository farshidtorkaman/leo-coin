using System;
using System.Globalization;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Factors.Queries
{
    public class SoldFactorVm : IMapFrom<Sell>
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public DateTime Created { get; set; }
        public PurchaseStatus Status { get; set; }
        public string TransactionLink { get; set; }
        public string RejectReason { get; set; }
        public string Description { get; set; }
        public string TrackingCode { get; set; }
        
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Sell, SoldFactorVm>()
                .ForMember(dst => dst.Amount, opt => opt.MapFrom(src => src.Amount.ToString(CultureInfo.InvariantCulture) + " " + src.Currency.Title));
        }
    }
}