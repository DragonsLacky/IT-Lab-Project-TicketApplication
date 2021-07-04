using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Enum;
using Ticket.Domain.Models;
using Ticket.Repository.Implementation;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<TicketModel> _ticketRepository;
        private readonly IRepository<TicketToCartModel> _ticketCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<TicketModel> ticketRepository, IRepository<TicketToCartModel> ticketCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketCartRepository = ticketCartRepository;
        }

        public ICollection<TicketModel> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public ICollection<TicketModel> GetTicketsByDate(DateTime date)
        {
            return _ticketRepository.GetAll().Where(ticket => ticket.ValidUntil.Date.Equals(date.Date)).ToList();
        }

        public ICollection<TicketModel> GetTicketsByGenre(Genre genre)
        {
            return _ticketRepository.GetAll().Where(ticket => ticket.Genre.Equals(genre)).ToList();
        }

        public ValueTask<TicketModel> GetTicket(Guid? id)
        {
            return _ticketRepository.Get(id);
        }

        public void CreateTicket(TicketModel ticket)
        {
            _ticketRepository.Insert(ticket);
            _ticketRepository.SaveChanges();
        }

        public void UpdateTicket(TicketModel ticket)
        {
            _ticketRepository.Update(ticket);
            _ticketRepository.SaveChanges();
        }

        public void DeleteTicket(TicketModel ticket)
        {
            _ticketRepository.Delete(ticket);
            _ticketRepository.SaveChanges();
        }

        public async ValueTask<bool> AddToCart(AddToCartDto ticketCartDto, string uid)
        {
            CartModel cart = _userRepository.GetCart(uid);

            if (cart == null)
            {
                throw new ArgumentNullException("Cart Does not Exist. Have you logged in?");
            }

            var ticket = await _ticketRepository.Get(ticketCartDto.TicketId);
            if (ticket.Seats - ticketCartDto.Quantity < 0)
            {
                return false;
            }

            TicketToCartModel ticketToAdd = new TicketToCartModel
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                Cart = cart,
                CartId = cart.Id,
                Quantity = ticketCartDto.Quantity
            };

            _ticketCartRepository.Insert(ticketToAdd);
            _ticketCartRepository.SaveChanges();
            return true;
        }
    }
}
