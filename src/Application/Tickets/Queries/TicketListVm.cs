using System;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Tickets.Queries
{
    public class TicketListVm : IMapFrom<Ticket>
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime Created { get; set; }
        public TicketStatus Status { get; set; }
    }
}