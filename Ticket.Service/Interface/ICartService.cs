using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;

namespace Ticket.Service.Interface
{
    public interface ICartService
    {
        CartDto GetCartTickets(string userId);
        bool RemoveFromCart(string userId, Guid id);
        ValueTask<bool> Buy(string userId);
    }
}
