using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Models;

namespace Ticket.Domain.DTO
{
    public class AddToCartDto
    {
        public TicketModel Ticket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
