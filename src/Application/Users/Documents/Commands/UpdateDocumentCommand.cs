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
        public IFormFile ApplicantImage { get; set; }
    }

    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IImageAccessor _imageAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public UpdateDocumentCommandHandler(IApplicationDbContext context, IImageAccessor imageAccessor,
            ICurrentUserService currentUserService, INotificationService notificationService)
        {
            _context = context;
            _imageAccessor = imageAccessor;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _context.Documents.FindAsync(request.Id);
            if (document == null)
            {
                document = new Document
                {
                    NationalCode = request.NationalCode,
                    BirthDate = request.BirthDate
                };
                if (request.NationalCardImage != null)
                {
                    document.NationalCardImage =
                        await _imageAccessor.Upload(request.NationalCardImage, _currentUserService.UserId,
                            "نصویر کارت ملی");
                    document.NationalCardImageStatus = DocumentImagesStatus.Sent;

                    _notificationService.SendAsync(NotificationType.NationalCard);
                }

                if (request.ApplicantImage != null)
                {
                    document.ApplicantImage =
                        await _imageAccessor.Upload(request.ApplicantImage, _currentUserService.UserId,
                            "نصویر درخواست نامه");
                    document.ApplicantImageStatus = DocumentImagesStatus.Sent;

                    _notificationService.SendAsync(NotificationType.Applicant);
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
                            await _imageAccessor.Upload(request.NationalCardImage, _currentUserService.UserId,
                                "تصویر کارت ملی");
                        document.NationalCardImageStatus = DocumentImagesStatus.Sent;

                        _notificationService.SendAsync(NotificationType.NationalCard);
                    }
                }

                if (document.ApplicantImageStatus == DocumentImagesStatus.Rejected ||
                    document.ApplicantImageStatus == null)
                {
                    if (request.ApplicantImage != null)
                    {
                        document.ApplicantImage =
                            await _imageAccessor.Upload(request.ApplicantImage, _currentUserService.UserId,
                                "تصویر درخواست نامه");
                        document.ApplicantImageStatus = DocumentImagesStatus.Sent;

                        _notificationService.SendAsync(NotificationType.Applicant);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}