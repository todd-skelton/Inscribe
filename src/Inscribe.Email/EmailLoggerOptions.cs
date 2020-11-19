using System;
using System.Collections.Generic;

namespace Inscribe.Email
{
    public class EmailLoggerOptions : IEmailLoggerOptions
    {
        public string ApplicationName { get; set; }

        public SmtpSettings Smtp { get; set; }

        public EmailAddressSettings From { get; set; }

        public IEnumerable<EmailAddressSettings> To { get; set; }

        public IEnumerable<EmailAddressSettings> CC { get; set; }

        public IEnumerable<EmailAddressSettings> Bcc { get; set; }

        public bool IncludeScopes { get; set; }

        public TimeSpan? ThrottleTimeout { get; set; }
    }
}
