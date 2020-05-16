using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;

namespace Crypto.Application.Users.Profile.Queries
{
    public class UserProfileVm : IMapFrom<UserProfile>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; }

        public string Tell { get; set; }
        
        public bool TellConfirmed { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<UserProfile, UserProfileVm>()
                .ForMember(dst => dst.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId == 0 ? null as int? : src.ProvinceId))
                .ForMember(dst => dst.CityId, opt => opt.MapFrom(src => src.CityId == 0 ? null as int? : src.CityId));
        }
    }
}
