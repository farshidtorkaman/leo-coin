using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IApplicationDbContext _context;

        public NotificationService(IApplicationDbContext context)
        {
            _context = context;
        }

        public void SendAsync(NotificationType type)
        {
            _context.Notifications.Add(new Notification {Type = type});
        }

        public void SendAsync(string text, string userId, NotificationType type = NotificationType.Simple)
        {
            _context.Notifications.Add(new Notification
            {
                Type = type,
                Text = text,
                UserId = userId
            });
        }
    }
}