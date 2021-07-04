using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticket.Domain.DTO;
using Ticket.Domain.Models;
using Ticket.Service.Implementation;
using Ticket.Service.Interface;

namespace Ticket.Web.Controllers
{
    public class TicketController : Controller
    {

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            return View(_ticketService.GetAllTickets());
        }

        [HttpPost]
        public IActionResult Index(DateTime term)
        {
            return View(_ticketService.GetTicketsByDate(term));
        }

        // GET: Tickets/AddCart/5
        public async Task<IActionResult> AddCart(Guid? id)
        {
            var ticket = await _ticketService.GetTicket(id);
            AddToCartDto model = new AddToCartDto
            {
                Quantity = 1,
                Ticket = ticket,
                TicketId = ticket.Id
            };
            return View(model);
        }

        // Post: Tickets/AddCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCart([Bind("TicketId,Quantity")] AddToCartDto model)
        {
            bool result = await _ticketService.AddToCart(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicket(id);

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,ValidUntil,Seats,Price,Genre,In3D")] TicketModel ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _ticketService.CreateTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicket(id);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,ValidUntil,Seats,Price,Genre,In3D")] TicketModel ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _ticketService.GetTicket(id);
            _ticketService.DeleteTicket(ticket);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _ticketService.GetAllTickets().Any(e => e.Id == id);
        }
    }
}
