using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _messageRepository;

        public EmailSender(IEmailService emailService, IRepository<EmailMessage> messageRepository)
        {
            _emailService = emailService;
            _messageRepository = messageRepository;
        }

        public async Task DoWork()
        {
            await _emailService.SendAllEmailAsync(_messageRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }
}
