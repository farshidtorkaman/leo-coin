using Crypto.Domain.Common;
using Crypto.Domain.Enums;

namespace Crypto.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}