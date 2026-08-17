 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phishing_Project.src.Models
{
    public class EmailMessage
    {
        public string Sender { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> AttachmentNames { get; set; } = new List<string>();
    }
}
