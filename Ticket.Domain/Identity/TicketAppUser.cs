using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Models;

namespace Ticket.Domain.Identity
{
    public class TicketAppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MyProperty { get; set; }
        public virtual CartModel Cart{ get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
