using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Identity;

namespace Ticket.Domain.Models
{
    public class CartModel : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public TicketAppUser Owner { get; set; }
        public string OwnerId { get; set; }
        public virtual ICollection<TicketToCartModel> Tickets { get; set; }
    }
}
