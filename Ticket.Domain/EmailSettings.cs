using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Domain
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public int  SmtpServerPort{ get; set; }
        public bool EnableSll { get; set; }
        public string EmailDisplayName { get; set; }
        public string SenderName { get; set; }

        public EmailSettings(string senderName, string emailDisplayName, bool enableSll, int smtpServerPort, string setPassword, string smtpUserName, string smtpServer)
        {
            SenderName = senderName;
            EmailDisplayName = emailDisplayName;
            EnableSll = enableSll;
            SmtpServerPort = smtpServerPort;
            SmtpPassword = setPassword;
            SmtpUserName = smtpUserName;
            SmtpServer = smtpServer;
        }

        public EmailSettings() { }
    }
}
