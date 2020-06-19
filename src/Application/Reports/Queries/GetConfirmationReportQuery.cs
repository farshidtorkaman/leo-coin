using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Reports.Queries
{
    public class GetConfirmationReportQuery : IRequest<List<ConfirmationQueryVm>>
    {
        public string UserId { get; set; }
    }

    public class
        GetConfirmationReportQueryHandler : IRequestHandler<GetConfirmationReportQuery, List<ConfirmationQueryVm>>
    {
        private readonly IApplicationDbContext _context;

        public GetConfirmationReportQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConfirmationQueryVm>> Handle(GetConfirmationReportQuery request,
            CancellationToken cancellationToken)
        {
            var profile =
                await _context.UsersProfiles.SingleOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);
            if (profile == null)
                throw new NotFoundException(nameof(Profile), request.UserId);

            var document =
                await _context.Documents.SingleOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            var phoneReport = new ConfirmationQueryVm {Title = "تلفن همراه"};
            if (profile.PhoneNumber != null)
            {
                phoneReport.Status = profile.PhoneNumberConfirmed switch
                {
                    null => Status.Sent,
                    true => Status.Confirmed,
                    _ => Status.Rejected
                };
                phoneReport.ClassName = profile.PhoneNumberConfirmed switch
                {
                    null => "badge-light",
                    true => "badge-success",
                    _ => "badge-danger"
                };
            }
            else
            {
                phoneReport.Status = Status.NotSent;
                phoneReport.ClassName = "";
            }

            var tellReport = new ConfirmationQueryVm {Title = "تلفن ثابت"};
            if (profile.Tell != null)
            {
                tellReport.Status = profile.TellConfirmed switch
                {
                    null => Status.Sent,
                    true => Status.Confirmed,
                    _ => Status.Rejected
                };
                tellReport.ClassName = profile.TellConfirmed switch
                {
                    null => "badge-light",
                    true => "badge-success",
                    _ => "badge-danger"
                };
            }
            else
            {
                tellReport.Status = Status.NotSent;
                tellReport.ClassName = "";
            }


            var nationalCardReport = new ConfirmationQueryVm {Title = "کارت ملی"};
            // var bankCardReport = new ConfirmationQueryVm {Title = "کارت بانکی"};
            var applicantReport = new ConfirmationQueryVm {Title = "درخواست نامه"};
            
            if (document != null)
            {
                if (document.NationalCardImage != null)
                {
                    nationalCardReport.Status = document.NationalCardImageStatus switch
                    {
                        DocumentImagesStatus.Sent => Status.Sent,
                        DocumentImagesStatus.Confirmed => Status.Confirmed,
                        _ => Status.Rejected
                    };
                    nationalCardReport.ClassName = document.NationalCardImageStatus switch
                    {
                        DocumentImagesStatus.Sent => "badge-light",
                        DocumentImagesStatus.Confirmed => "badge-success",
                        _ => "badge-danger"
                    };
                }
                else
                {
                    nationalCardReport.Status = Status.NotSent;
                    nationalCardReport.ClassName = "";
                }

                // if (document.BankCardImage != null)
                // {
                //     bankCardReport.Status = document.BankCardImageStatus switch
                //     {
                //         DocumentImagesStatus.Sent => Status.Sent,
                //         DocumentImagesStatus.Confirmed => Status.Confirmed,
                //         _ => Status.Rejected
                //     };
                //     bankCardReport.ClassName = document.BankCardImageStatus switch
                //     {
                //         DocumentImagesStatus.Sent => "badge-light",
                //         DocumentImagesStatus.Confirmed => "badge-success",
                //         _ => "badge-danger"
                //     };
                // }
                // else
                // {
                //     bankCardReport.Status = Status.NotSent;
                //     bankCardReport.ClassName = "";
                // }

                if (document.ApplicantImage != null)
                {
                    applicantReport.Status = document.ApplicantImageStatus switch
                    {
                        DocumentImagesStatus.Sent => Status.Sent,
                        DocumentImagesStatus.Confirmed => Status.Confirmed,
                        _ => Status.Rejected
                    };
                    applicantReport.ClassName = document.ApplicantImageStatus switch
                    {
                        DocumentImagesStatus.Sent => "badge-light",
                        DocumentImagesStatus.Confirmed => "badge-success",
                        _ => "badge-danger"
                    };
                }
                else
                {
                    applicantReport.Status = Status.NotSent;
                    applicantReport.ClassName = "";
                }
            }
            else
            {
                nationalCardReport.Status = Status.NotSent;
                nationalCardReport.ClassName = "";

                // bankCardReport.Status = Status.NotSent;
                // bankCardReport.ClassName = "";

                applicantReport.Status = Status.NotSent;
                applicantReport.ClassName = "";
            }


            return new List<ConfirmationQueryVm>
                {phoneReport, tellReport, nationalCardReport, applicantReport};
        }
    }
}