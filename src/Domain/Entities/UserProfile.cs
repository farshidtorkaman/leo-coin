using System;
using System.Collections.Generic;
using System.Text;
using Crypto.Domain.Common;

namespace Crypto.Domain.Entities
{
    public class UserProfile : AuditableEntity
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        
        public bool? PhoneNumberConfirmed { get; set; }

        public string Tell { get; set; }
        
        public bool? TellConfirmed { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }
    }
}
