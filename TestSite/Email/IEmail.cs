using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestSite.Email
{
    interface IEmail
    {
        void Send(MailMessage msg);
    }
}
