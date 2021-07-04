using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain;
using Ticket.Domain.Models;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendAllEmailAsync(List<EmailMessage> mails)
        {
            List<MimeMessage> messages = new List<MimeMessage>();
            foreach (var mail in mails)
            {
                var emailMessage = new MimeMessage
                {
                    Sender = new MailboxAddress(_settings.SenderName, _settings.SmtpUserName),
                    Subject = mail.Subject,
                    Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = mail.Content }
                };
                emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUserName));
                emailMessage.To.Add(new MailboxAddress(mail.To));
                messages.Add(emailMessage);
            }

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = _settings.EnableSll ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                    await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpServerPort, socketOptions);

                    if (string.IsNullOrEmpty(_settings.SmtpUserName))
                    {
                        await smtp.AuthenticateAsync(_settings.SmtpUserName, _settings.SmtpPassword);
                    }

                    foreach (var message in messages)
                    {
                        await smtp.SendAsync(message);
                    }

                    await smtp.DisconnectAsync(true);
                }
            }
            catch(SmtpException e)
            {
                throw e;
            }
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.SenderName, _settings.SmtpUserName),
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message.Content }
            };

            emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUserName));
            emailMessage.To.Add(new MailboxAddress(message.To));

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = _settings.EnableSll ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                    await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpServerPort, socketOptions);

                    if (!string.IsNullOrEmpty(_settings.SmtpUserName))
                    {
                        await smtp.AuthenticateAsync(_settings.SmtpUserName, _settings.SmtpPassword);
                    }

                    await smtp.SendAsync(emailMessage);

                    await smtp.DisconnectAsync(true);
                }
            }
            catch (SmtpException e)
            {
                throw e;
            }
        }
    }
}
