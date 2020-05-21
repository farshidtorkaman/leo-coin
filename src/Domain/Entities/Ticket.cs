using System.Collections.Generic;
using Crypto.Domain.Common;
using Crypto.Domain.Enums;

namespace Crypto.Domain.Entities
{
    public sealed class Ticket : AuditableEntity
    {
        public Ticket()
        {
            Messages = new List<TicketMessage>();
        }
        public int Id { get; set; }
        public string Topic { get; set; }
        public List<TicketMessage> Messages { get; set; }
        public TicketStatus Status { get; set; }
    }
}