using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.Identity;
using Ticket.Domain.Models;

namespace Ticket.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketAppUser> GetAll();
        ValueTask<TicketAppUser> Get(string id);
        TicketAppUser GetByEmail(string email);
        void Insert(TicketAppUser entity);
        void Update(TicketAppUser entity);
        void Delete(TicketAppUser entity);
        public CartModel GetCart(string id);
        public ICollection<Order> GetOrders(string id);
        public void saveChanges();
    }
}
