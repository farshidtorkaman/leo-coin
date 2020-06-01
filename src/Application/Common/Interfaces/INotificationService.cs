using Crypto.Domain.Enums;

namespace Crypto.Application.Common.Interfaces
{
    public interface INotificationService
    {
        void SendAsync(NotificationType type);
        void SendAsync(string text, string userId, NotificationType type);
    }
}