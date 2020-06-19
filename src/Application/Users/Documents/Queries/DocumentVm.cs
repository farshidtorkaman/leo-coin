using System;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Users.Documents.Queries
{
    public class DocumentVm : IMapFrom<Document>
    {
        public int Id { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCardImage { get; set; }
        public string ApplicantImage { get; set; }
        public DocumentImagesStatus? NationalCardImageStatus { get; set; }
        public DocumentImagesStatus? ApplicantImageStatus { get; set; }
    }
}