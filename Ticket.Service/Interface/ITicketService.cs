using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Enum;
using Ticket.Domain.Models;

namespace Ticket.Service.Interface
{
    public interface ITicketService
    {
        ICollection<TicketModel> GetAllTickets();
        ICollection<TicketModel> GetTicketsByDate(DateTime date);
        public ICollection<TicketModel> GetTicketsByGenre(Genre genre);
        ValueTask<TicketModel> GetTicket(Guid? id);
        void CreateTicket(TicketModel ticket);
        void UpdateTicket(TicketModel ticket);
        void DeleteTicket(TicketModel ticket);
        public ValueTask<bool> AddToCart(AddToCartDto ticketCartDto, string uid);
    }
}
