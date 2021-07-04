using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public OrderDto GetOrder(Guid id)
        {
            return _orderRepository.GetTicketsForOrder(id);
        }

        public ICollection<Order> GetOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public ICollection<Order> GetUserOrders(string id)
        {
            return _userRepository.GetOrders(id);
        }
    }
}
