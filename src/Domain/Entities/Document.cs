using System;
using Crypto.Domain.Common;
using Crypto.Domain.Enums;

namespace Crypto.Domain.Entities
{
    public class Document : AuditableEntity
    {
        public int Id { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCardImage { get; set; }
        public string ApplicantImage { get; set; }
        public string UserId { get; set; }
        public DocumentImagesStatus? NationalCardImageStatus { get; set; }
        public DocumentImagesStatus? ApplicantImageStatus { get; set; }
    }
}