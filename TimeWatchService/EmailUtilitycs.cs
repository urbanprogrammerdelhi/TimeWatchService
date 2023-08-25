using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TimeWatchService
{
    public class EmailUtility
    {
        public static void SendMail(MessageBody mailMessage)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                     | SecurityProtocolType.Tls11
                                     | SecurityProtocolType.Tls12;

                MailMessage mm = new MailMessage(mailMessage.SmTpDetails.Sender, mailMessage.Receiver);
                mm.Subject = mailMessage.EmailSubject;
                mm.Body = mailMessage.EmailBody;
                mm.IsBodyHtml = true;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailMessage.SmTpDetails.SmtpHost;
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mailMessage.SmTpDetails.Credentials[0];
                NetworkCred.Password = mailMessage.SmTpDetails.Credentials[1];
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = mailMessage.SmTpDetails.SmtpPort.ParseToInteger();
                smtp.Send(mm);
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex);
            }
        }
    }
}
