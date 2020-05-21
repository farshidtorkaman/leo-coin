using System;
using System.Collections.Generic;
using AutoMapper;
using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;

namespace Crypto.Application.Tickets.Queries
{
    public class TicketDetailsVm : IMapFrom<Ticket>
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime Created { get; set; }
        public TicketStatus Status { get; set; }
        public bool UserCanNotReply { get; set; }
        public List<TicketMessageVm> Messages { get; set; }
    }

    public class TicketMessageVm : IMapFrom<TicketMessage>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}