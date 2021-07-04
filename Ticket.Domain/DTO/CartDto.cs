using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket.Domain.DTO
{
    public class CartDto
    {
        public ICollection<CartItemDto> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
