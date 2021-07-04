using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;

namespace Ticket.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> _entities;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Order>();
        }

        public ICollection<Order> GetAllOrders()
        {
            return _entities
                .Include(order => order.User)
                .Include(order => order.Tickets)
                .Include("Tickets.Ticket")
                .ToList();
        }

        public OrderDto GetTicketsForOrder(Guid id)
        {
            Order order = _entities
                .Include(order => order.User)
                .Include(order => order.Tickets)
                .Include("Tickets.Ticket")
                .SingleOrDefault(order => order.Id == id);

            return new OrderDto {
                Id = order.Id,
                TotalPrice = order.Tickets.Select(z => z.Quantity * z.Ticket.Price).Sum(),
                Tickets = order.Tickets
                .Select(ticket => new OrderItemDto
                {
                    Quantity = ticket.Quantity,
                    Ticket = ticket.Ticket
                }).ToList()
            };
        }
    }
}
