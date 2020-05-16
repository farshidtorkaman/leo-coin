using System;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Crypto.Application.Users.Documents.Commands
{
    public class UpdateDocumentCommand : IRequest
    {
        public int Id { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile NationalCardImage { get; set; }
        public IFormFile BankCardImage { get; set; }
        public IFormFile ApplicantImage { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IImageAccessor _imageAccessor;

        public UpdateDocumentCommandHandler(IApplicationDbContext context, IImageAccessor imageAccessor)
        {
            _context = context;
            _imageAccessor = imageAccessor;
        }

        public async Task<Unit> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _context.Documents.FindAsync(request.Id);
            if (document == null)
            {
                document = new Document
                {
                    NationalCode = request.NationalCode,
                    BirthDate = request.BirthDate,
                    UserId = request.UserId
                };
                if (request.NationalCardImage != null)
                {
                    document.NationalCardImage =
                        await _imageAccessor.Upload(request.NationalCardImage, request.UserId, "نصویر کارت ملی");
                    document.NationalCardImageStatus = DocumentImagesStatus.Sent;
                }

                if (request.BankCardImage != null)
                {
                    document.BankCardImage =
                        await _imageAccessor.Upload(request.BankCardImage, request.UserId, "نصویر کارت بانکی");
                    document.BankCardImageStatus = DocumentImagesStatus.Sent;
                }

                if (request.ApplicantImage != null)
                {
                    document.ApplicantImage =
                        await _imageAccessor.Upload(request.ApplicantImage, request.UserId, "نصویر درخواست نامه");
                    document.ApplicantImageStatus = DocumentImagesStatus.Sent;
                }

                _context.Documents.Add(document);
            }
            else
            {
                if (document.NationalCardImageStatus == DocumentImagesStatus.Rejected ||
                    document.NationalCardImageStatus == null)
                {
                    document.NationalCode = request.NationalCode;
                    document.BirthDate = request.BirthDate;
                    
                    if (request.NationalCardImage != null)
                    {
                        document.NationalCardImage =
                            await _imageAccessor.Upload(request.NationalCardImage, request.UserId, "تصویر کارت ملی");
                        document.NationalCardImageStatus = DocumentImagesStatus.Sent;
                    }
                }


                if (document.BankCardImageStatus == DocumentImagesStatus.Rejected ||
                    document.BankCardImageStatus == null)
                {
                    if (request.BankCardImage != null)
                    {
                        document.BankCardImage =
                            await _imageAccessor.Upload(request.BankCardImage, request.UserId, "تصویر کارت بانکی");
                        document.BankCardImageStatus = DocumentImagesStatus.Sent;
                    }
                }


                if (document.ApplicantImageStatus == DocumentImagesStatus.Rejected ||
                    document.ApplicantImageStatus == null)
                {
                    if (request.ApplicantImage != null)
                    {
                        document.ApplicantImage =
                            await _imageAccessor.Upload(request.ApplicantImage, request.UserId, "تصویر درخواست نامه");
                        document.ApplicantImageStatus = DocumentImagesStatus.Sent;
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}