using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Domain.Models
{
    public class OrderedTickets : BaseEntity
    {
        public Guid TicketId { get; set; }
        public TicketModel Ticket { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
