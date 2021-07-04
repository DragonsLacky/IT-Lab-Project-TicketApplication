using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Identity;

namespace Ticket.Domain.Models
{
    public class Order : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public TicketAppUser User{ get; set; }
        public DateTime Completed { get; set; }
        public virtual ICollection<OrderedTickets> Tickets { get; set; }
    }
}
