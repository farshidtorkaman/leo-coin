using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;

namespace Crypto.Application.Cities.Queries
{
    public class CityVm : IMapFrom<City>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}