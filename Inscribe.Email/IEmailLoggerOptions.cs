using System.Collections.Generic;

namespace Inscribe.Email
{
    public interface IEmailLoggerOptions : ILoggerOptions
    {
        string ApplicationName { get; }

        SmtpSettings Smtp { get; }

        EmailAddressSettings From { get; }

        IEnumerable<EmailAddressSettings> To { get; }

        IEnumerable<EmailAddressSettings> CC { get; }

        IEnumerable<EmailAddressSettings> Bcc { get; }
    }
}
