using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TestSite.Email
{
    public class StandardSMTPEmailer : IEmail
    {
        public void Send(System.Net.Mail.MailMessage msg)
        {
            string email = ConfigurationManager.AppSettings["webEmail"];
            string password = ConfigurationManager.AppSettings["webEmailPassword"];
            string host = ConfigurationManager.AppSettings["webEmailHost"];

            // can include display name if needed
            msg.From = new MailAddress(email, "Real Estate");

            /*SmtpClient client = new SmtpClient();

            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(email, password);

            client.Credentials = new System.Net.NetworkCredential("reiirealestateweb@gmail.com", "reiirealestateweb@gmail.com", "gmail.com");

            client.Port = 587;
            client.Host = host;
            client.EnableSsl = true;
            

            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            client.Send(msg);*/

            var client = new SmtpClient(host, 587)
            {
                Credentials = new System.Net.NetworkCredential(email, password),
                EnableSsl = true
            };
            client.Send(msg);
            //client.Send(email, "stefanjacholke@gmail.com", "test", "testbody");

        }
    }
}