using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Inscribe.Email
{
    public class EmailEntryProcessor : EntryProcessorBase<MailMessage>
    {
        public string ApplicationName;
        public SmtpClient Client;
        public MailAddress From;
        public IEnumerable<MailAddress> To;
        public IEnumerable<MailAddress> CC;
        public IEnumerable<MailAddress> Bcc;
        public TimeSpan? ThrottleTimeout;

        public EmailEntryProcessor() : base("Email logger entry processing thread", 32)
        {

        }

        protected override void WriteEntry(MailMessage entry)
        {
            try
            {
                Client.Send(entry);
            }
            catch { }
        }
    }
}
