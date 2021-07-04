using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ticket.Domain.DTO;
using Ticket.Domain.Identity;
using Ticket.Domain.Models;
using Ticket.Repository;
using Ticket.Service.Implementation;
using Ticket.Service.Interface;

namespace Ticket.Web.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService _cartService;


        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var model = _cartService.GetCartTickets(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(model);
        }


        public async Task<IActionResult> RemoveCart(Guid id)
        {
            _cartService.RemoveFromCart(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Pay(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var order = _cartService.GetCartTickets(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(order.TotalPrice)*100,
                Description = "Tickets Payment",
                Currency = "mkd",
                Customer = customer.Id
            });

            if(charge.Status == "succeeded")
            {
                var result = await CompleteOrder();
                if (result)
                {
                    return RedirectToAction("Index", "Ticket");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        private async Task<bool> CompleteOrder()
        {
            return await _cartService.Buy(User.FindFirstValue(ClaimTypes.NameIdentifier));
             
        }
    }
}
