using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ticket.Domain.Enum;

namespace Ticket.Domain.Models
{
    public class TicketModel : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DisplayName("Starts")]
        public DateTime ValidUntil { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [DisplayName("3D")]
        public bool In3D { get; set; }
        [Required]
        [DisplayName("Seats")]
        public int Seats { get; set; }
        public Genre Genre { get; set; }
        public virtual ICollection<TicketToCartModel> Carts { get; set; }
        public virtual ICollection<OrderedTickets> Orders { get; set; }
    }
}
