using System.Collections.Generic;

namespace Crypto.Domain.Entities
{
    public class Province
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<City> Cities { get; set; }
    }
}
