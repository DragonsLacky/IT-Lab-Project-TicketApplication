using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.Models;

namespace Ticket.Service.Interface
{
    public interface IEmailService
    {
        public Task SendAllEmailAsync(List<EmailMessage> mails);
        public Task SendEmailAsync(EmailMessage message);
    }
}
