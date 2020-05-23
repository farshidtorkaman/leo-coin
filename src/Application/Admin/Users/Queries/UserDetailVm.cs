using Crypto.Domain.Enums;

namespace Crypto.Application.Admin.Users.Queries
{
    public class UserDetailVm
    {
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
        public string BankCardNumber { get; set; }
        public string BankName { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
    }
}