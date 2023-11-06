using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ApptHeroAPI.Common.Models
{
    public class EmailModel
    {
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string ReplyToListEmail { get; set; }
        public string ReplyToListName { get; set; }
        
    }
}
