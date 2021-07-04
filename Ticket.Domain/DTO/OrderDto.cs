using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Domain.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public ICollection<OrderItemDto> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
