using Crypto.Domain.Common;

namespace Crypto.Domain.Entities
{
    public class TicketMessage : AuditableEntity
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string Text { get; set; }
    }
}