using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;

namespace Ticket.Service.Interface
{
    public interface IOrderService
    {
        ICollection<Order> GetOrders();
        OrderDto GetOrder(Guid id);
        ICollection<Order> GetUserOrders(string id);
    }
}
