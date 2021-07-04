using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Domain.Models
{
    public class TicketToCartModel : BaseEntity
    {
        public Guid TicketId { get; set; }
        public TicketModel Ticket { get; set; }
        public Guid CartId { get; set; }
        public CartModel Cart { get; set; }
        public int Quantity { get; set; }
    }
}
