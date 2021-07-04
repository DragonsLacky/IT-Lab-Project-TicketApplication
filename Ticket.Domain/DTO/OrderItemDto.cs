using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.Models;

namespace Ticket.Domain.DTO
{
    public class OrderItemDto
    {
        public TicketModel Ticket { get; set; }
        public int Quantity { get; set; }
    }
}
