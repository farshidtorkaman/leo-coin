using System;
using AutoMapper;
using Crypto.Application.Common;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Admin.Notifications.Queries
{
    public class NotificationVm
    {
        public int Id { get; set; }
        public string UsersFullName { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
        public string CreatedBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Notification, NotificationVm>()
                .ForMember(dst => dst.Text,
                    opt => opt.MapFrom(src =>
                        src.Type == NotificationType.Applicant ? "ثبت درخواست نامه" :
                        src.Type == NotificationType.Purchase ? "ثبت خرید" :
                        src.Type == NotificationType.Sell ? "ثبت فروش" :
                        src.Type == NotificationType.Tell ? "ثبت تلفن ثابت" :
                        src.Type == NotificationType.Ticket ? "ثبت تیکت" :
                        src.Type == NotificationType.BankCard ? "ثبت کارت بانکی" :
                        src.Type == NotificationType.NationalCard ? "ثبت کارت ملی" :
                        ""
                    ));
            // .ForMember(dst => dst.UsersFullName,
            //     opt => opt.MapFrom(src => _identityService.GetFullName(src.CreatedBy)));
        }
    }
}