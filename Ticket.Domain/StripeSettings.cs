using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Domain
{
    public class StripeSettings
    {
        public string Publishable { get; set; }
        public string Secret { get; set; }
    }
}
