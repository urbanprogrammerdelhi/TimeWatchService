using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWatchService
{
    public class ApplicationConstants
    {
        public static readonly string TimeWatchServiceEmailFormat = "<html>\r\n<span>\r\n    Dear Sir / Madam,\r\n    <br />\r\n    <span>\r\n  There was an issue while running the Time Watch Service. Please find it . @Exception      Sincerely,\r\n    </span>\r\n    <br />\r\n    @SenderName\r\n</span>\r\n</html>\r\n";

    }
    public static class Extensions
    {

        public static int ParseToInteger(this string str)
        {
            if (int.TryParse(str, out int result)) return result;
            return 0;
        }
        public static int ParseToInteger(this Object obj)
        {
            if (obj == null) return -1;
            if (int.TryParse(obj.ParseToText(), out int result)) return result;
            return -1;
        }
        public static string ParseToText(this Object obj)
        {
            if (obj == null)
                return string.Empty;
            return Convert.ToString(obj);
        }
        public static void SendExceptionMail(this Exception exception)
        {
            try
            {
                var message = new MessageBody
                {
                    Attachment = string.Empty,
                    AttachmentName = string.Empty,
                    EmailBody = string.Empty,
                    Receiver = ConfigurationManager.AppSettings["AdminEmails"].ParseToText()
                };
                message.EmailSubject = ConfigurationManager.AppSettings["Subject"].ParseToText();
                string userName = ConfigurationManager.AppSettings["UserName"].ParseToText();
                string password = ConfigurationManager.AppSettings["Password"].ParseToText();
                message.SmTpDetails = new SmTpDetails
                {
                    Credentials = new string[] { userName, password },
                    Sender = ConfigurationManager.AppSettings["Sender"].ParseToText(),
                    SmtpHost = ConfigurationManager.AppSettings["SmtpServer"].ParseToText(),
                    SmtpPort = ConfigurationManager.AppSettings["Port"].ParseToText(),
                    SenderName = ConfigurationManager.AppSettings["SenderName"].ParseToText()

                };
                string emailBody = ApplicationConstants.TimeWatchServiceEmailFormat;
                emailBody = emailBody.Replace("@SenderName", message.SmTpDetails.SenderName);
                emailBody = emailBody.Replace("@Exception", $"{exception.Message}{Environment.NewLine}{exception.StackTrace}");
                message.EmailBody = emailBody;
                EmailUtility.SendMail(message);
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex);
            }
        }

    }
}
