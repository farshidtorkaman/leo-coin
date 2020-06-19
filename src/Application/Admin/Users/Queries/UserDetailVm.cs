using System.Collections.Generic;
using Crypto.Domain.Enums;

namespace Crypto.Application.Admin.Users.Queries
{
    public class UserDetailVm
    {
        public UserDetailVm()
        {
            Financials = new List<Financial>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Tell { get; set; }
        public Status TellStatus { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
        public string NationalCardImage { get; set; }
        public Status NationalCardStatus { get; set; }
        public string BankCardImage { get; set; }
        public Status BankCardStatus { get; set; }
        public string ApplicantImage { get; set; }
        public Status ApplicantStatus { get; set; }
        public List<Financial> Financials { get; set; }
    }

    public class Financial
    {
        public int Id { get; set; }
        public string BankCardNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
        public string BankCardImage { get; set; }
        public Status Status { get; set; }
    }
}