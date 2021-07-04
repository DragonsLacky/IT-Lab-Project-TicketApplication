using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;
using Ticket.Repository.Implementation;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartModel> _cartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<OrderedTickets> _orderedTicketsRepository;
        private readonly IRepository<EmailMessage> _emailMessagesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public CartService(IRepository<CartModel> cartRepositorty, IRepository<OrderedTickets> orderedTicketsRepository, IRepository<Order> orderRepositorty, IUserRepository userRepository, IRepository<EmailMessage> emailMessagesRepository, IEmailService emailService)
        {
            _cartRepositorty = cartRepositorty;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _orderedTicketsRepository = orderedTicketsRepository;
            _emailMessagesRepository = emailMessagesRepository;
            _emailService = emailService;
        }

        public async ValueTask<bool> Buy(string uid)
        {
            var user = await _userRepository.Get(uid);
            var cart = _userRepository.GetCart(uid);

            if (cart.Tickets.Count == 0)
            {
                return false;
            }

            EmailMessage message = new EmailMessage
            {
                Id = Guid.NewGuid(),
                To = user.Email,
                Subject = "Order Summary",
                Status = false 
            };

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Transaction Completed. Order Contents: ");


            Order order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = uid,
                Completed = DateTime.Now,
                User = user
            };

            _orderRepositorty.Insert(order);
            _orderRepositorty.SaveChanges();

            var total = cart.Tickets.Select(ticket => ticket.Quantity * ticket.Ticket.Price).Sum();

            cart.Tickets
                .Select(z => {
                    cart.Tickets.Remove(z);
                    return new OrderedTickets
                    {
                        Order = order,
                        OrderId = order.Id,
                        Ticket = z.Ticket,
                        TicketId = z.TicketId,
                        Quantity = z.Quantity
                    };
                })
                .ToList().ForEach(orderToTicket => {
                    builder.Append(orderToTicket.Ticket.Title).Append(" ").Append(orderToTicket.Ticket.Price).Append(" x ").AppendLine(orderToTicket.Quantity.ToString()) ;
                    _orderedTicketsRepository.Insert(orderToTicket); 
                });

            builder.Append("Your Total is: ").AppendLine(total.ToString());
            message.Content = builder.ToString();

            _emailMessagesRepository.Insert(message);
            _emailMessagesRepository.SaveChanges();

            await _emailService.SendEmailAsync(message);

            _orderedTicketsRepository.SaveChanges();
            _cartRepositorty.SaveChanges();
            return true;
        }

        public CartDto GetCartTickets(string uid)
        {
            var cart = _userRepository.GetCart(uid);

            if (cart == null)
            {
                throw new ArgumentNullException("Cart not found, are you logged in?");
            }

            return new CartDto
            {
                Tickets = cart.Tickets.Select(z => new CartItemDto
                {
                    Ticket = z.Ticket,
                    Quantity = z.Quantity
                }).ToList(),
                TotalPrice = cart.Tickets.Select(z => z.Quantity * z.Ticket.Price).Sum(),
            }; 
        }

        public bool RemoveFromCart(string uid, Guid id)
        {
            var cart = _userRepository.GetCart(uid);

            if(cart == null)
            {
                return false;
            }

            cart.Tickets.Remove(cart.Tickets.Where(ticket => ticket.TicketId == id).FirstOrDefault());

            _cartRepositorty.Update(cart);
            _cartRepositorty.SaveChanges();

            return true;
        }
    }
}
