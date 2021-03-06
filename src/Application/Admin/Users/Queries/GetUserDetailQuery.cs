﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Enums;
using MD.PersianDateTime.Standard;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Users.Queries
{
    public class GetUserDetailQuery : IRequest<UserDetailVm>
    {
        public string UserId { get; set; }
    }

    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetUserDetailQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<UserDetailVm> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var email = await _identityService.GetUserNameAsync(request.UserId);
            if (email == null)
                throw new NotFoundException();

            var user = new UserDetailVm {Email = email};

            var profile =
                await _context.UsersProfiles.Include(f => f.Province).Include(f => f.City)
                    .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.PhoneNumber = profile.PhoneNumber ?? "-";
            user.Tell = profile.Tell ?? "-";
            user.TellStatus = profile.Tell == null ? Status.NotSent :
                profile.TellConfirmed == null ? Status.Sent :
                profile.TellConfirmed == true ? Status.Confirmed : Status.Rejected;
            user.Province = profile.Province?.Title ?? "-";
            user.City = profile.City?.Title ?? "-";
            user.Address = profile.Address ?? "-";
            user.PostalCode = profile.PostalCode ?? "-";

            var document =
                await _context.Documents.FirstOrDefaultAsync(f => f.CreatedBy == request.UserId, cancellationToken);
            user.NationalCode = document?.NationalCode ?? "-";
            user.BirthDate = document?.BirthDate != null
                ? new PersianDateTime(document.BirthDate).ToShortDateString()
                : "-";

            user.NationalCardImage = document?.NationalCardImage ?? "-";
            user.NationalCardStatus = document?.NationalCardImage == null ? Status.NotSent :
                document.NationalCardImageStatus == DocumentImagesStatus.Sent ? Status.Sent :
                document.NationalCardImageStatus == DocumentImagesStatus.Confirmed ? Status.Confirmed : Status.Rejected;

            // user.BankCardImage = document?.BankCardImage ?? "-";
            // user.BankCardStatus = document?.BankCardImage == null ? Status.NotSent :
            //     document.BankCardImageStatus == DocumentImagesStatus.Sent ? Status.Sent :
            //     document.BankCardImageStatus == DocumentImagesStatus.Confirmed ? Status.Confirmed : Status.Rejected;

            user.ApplicantImage = document?.ApplicantImage ?? "-";
            user.ApplicantStatus = document?.ApplicantImage == null ? Status.NotSent :
                document.ApplicantImageStatus == DocumentImagesStatus.Sent ? Status.Sent :
                document.ApplicantImageStatus == DocumentImagesStatus.Confirmed ? Status.Confirmed : Status.Rejected;

            var financials =
                await _context.FinancialInformation.Include(f => f.Bank).Where(f => f.CreatedBy == request.UserId)
                    .ToListAsync(cancellationToken);
            user.Financials = financials.Select(financial => new Financial
            {
                Id = financial.Id,
                BankCardNumber = financial.CardNumber,
                BankName = financial.Bank.Title,
                AccountNumber = financial.AccountNumber,
                Sheba = financial.Sheba,
                BankCardImage = financial.BankCardImage,
                Status = financial.Status
            }).ToList();

            return user;
        }
    }
}