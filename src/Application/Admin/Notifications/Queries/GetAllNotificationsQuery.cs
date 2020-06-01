using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Notifications.Queries
{
    public class GetAllNotificationsQuery : IRequest<List<NotificationVm>>
    {
    }

    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, List<NotificationVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetAllNotificationsQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<List<NotificationVm>> Handle(GetAllNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = await _context.Notifications.OrderByDescending(f => f.Created)
                .ToListAsync(cancellationToken);

            return notifications.Select(notification => new NotificationVm
            {
                Created = notification.Created,
                Id = notification.Id,
                Text = notification.Type switch
                {
                    NotificationType.Applicant => "ثبت درخواست نامه",
                    NotificationType.Purchase => "ثبت خرید",
                    NotificationType.Sell => "ثبت فروش",
                    NotificationType.Tell => "ثبت تلفن ثابت",
                    NotificationType.Ticket => "ثبت تیکت",
                    NotificationType.BankCard => "ثبت کارت بانکی",
                    NotificationType.NationalCard => "ثبت کارت ملی",
                    _ => ""
                },
                UsersFullName = _identityService.GetFullName(notification.CreatedBy),
                Type = notification.Type,
                CreatedBy = notification.CreatedBy
            }).ToList();
        }
    }
}