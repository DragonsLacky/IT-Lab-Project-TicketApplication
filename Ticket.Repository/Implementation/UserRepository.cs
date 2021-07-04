using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.Identity;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;

namespace Ticket.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TicketAppUser> _entities;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<TicketAppUser>();
        }

        public IEnumerable<TicketAppUser> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public ValueTask<TicketAppUser> Get(string id)
        {
            return _entities.FindAsync(id);
        }

        public TicketAppUser GetByEmail(string email)
        {
            return _entities.Where(u => u.Email == email).FirstOrDefault();
        }

        public void Insert(TicketAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
        }

        public void Update(TicketAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
        }

        public void Delete(TicketAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
        }

        public CartModel GetCart(string id)
        {
            return _entities
                .Include(user => user.Cart)
                .Include(user => user.Cart.Tickets)
                .Include("Cart.Tickets.Ticket")
                .SingleOrDefault(u => u.Id == id).Cart;
        }

        public ICollection<Order> GetOrders(string id)
        {
            return _entities
                .Include(user => user.Orders)
                .Include("Orders.Tickets")
                .Include("Orders.Tickets.Ticket")
                .SingleOrDefault(u => u.Id == id).Orders;
        }

        public void saveChanges()
        {
            _context.SaveChangesAsync();
        }

    }
}
