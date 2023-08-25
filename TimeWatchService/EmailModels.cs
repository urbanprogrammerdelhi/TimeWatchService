using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWatchService
{
    public class SmTpDetails
    {
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string[] Credentials { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
    }
    public class MessageBody
    {
        public string Attachment { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string Receiver { get; set; }
        public SmTpDetails SmTpDetails { get; set; }
        public string AttachmentName { get; set; }

    }
}
