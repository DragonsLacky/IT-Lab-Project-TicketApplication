using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain;
using Ticket.Domain.Identity;
using Ticket.Domain.Models;

namespace Ticket.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TicketModel> Tickets { get; set; }

        public virtual DbSet<CartModel> Carts { get; set; }

        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        public virtual DbSet<TicketToCartModel> TicketsToCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TicketModel>().Property(ticket => ticket.Id).ValueGeneratedOnAdd();

            builder.Entity<CartModel>().Property(cart => cart.Id).ValueGeneratedOnAdd();

            builder.Entity<Order>().Property(order => order.Id).ValueGeneratedOnAdd();

            builder.Entity<TicketToCartModel>().HasKey(ticketToCart => new { ticketToCart.TicketId, ticketToCart.CartId });

            builder.Entity<TicketToCartModel>()
                .HasOne(ticketToCart => ticketToCart.Ticket)
                .WithMany(ticket => ticket.Carts)
                .HasForeignKey(ticketToCart => ticketToCart.TicketId);

            builder.Entity<TicketToCartModel>()
               .HasOne(ticketToCart => ticketToCart.Cart)
               .WithMany(cart => cart.Tickets)
               .HasForeignKey(ticketToCart => ticketToCart.CartId);

            builder.Entity<CartModel>()
                .HasOne(cart => cart.Owner)
                .WithOne(user => user.Cart)
                .HasForeignKey<CartModel>(u => u.OwnerId);

            //builder.Entity<Order>()
            //    .HasOne(order => order.User)
            //    .WithMany(user => user.Orders)
            //    .HasForeignKey(u => u.UserId);

            builder.Entity<OrderedTickets>().HasKey(orderedTickets => new { orderedTickets.TicketId, orderedTickets.OrderId });

            builder.Entity<OrderedTickets>()
                .HasOne(orderedTickets => orderedTickets.Ticket)
                .WithMany(ticket => ticket.Orders)
                .HasForeignKey(orderedTickets => orderedTickets.TicketId);

            builder.Entity<OrderedTickets>()
                .HasOne(orderedTickets => orderedTickets.Order)
                .WithMany(ticket => ticket.Tickets)
                .HasForeignKey(orderedTickets => orderedTickets.OrderId);
        }
    }
}
