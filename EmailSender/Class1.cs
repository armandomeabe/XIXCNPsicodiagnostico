using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public static class Sender
    {
        public static void Send(string toAddress, string subject, string message)
        {
            var mail = new MailMessage("Congreso_ADEIP@appliedchaos.net", toAddress);
            var client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "applied01"
            };
            mail.Subject = subject;
            mail.Body = message;

            client.Send(mail);
        }
    }
}
