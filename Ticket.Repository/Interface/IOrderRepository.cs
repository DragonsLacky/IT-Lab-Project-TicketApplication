using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;

namespace Ticket.Repository.Interface
{
    public interface IOrderRepository
    {
        ICollection<Order> GetAllOrders();
        OrderDto GetTicketsForOrder(Guid id);
    }
}
