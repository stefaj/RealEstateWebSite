using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestSite.Email
{
    /// <summary>
    /// Interface for sending mail messages
    /// </summary>
    interface IEmail
    {
        /// <summary>
        /// Sends a mail message
        /// </summary>
        /// <param name="msg"></param>
        void Send(MailMessage msg);
    }
}
