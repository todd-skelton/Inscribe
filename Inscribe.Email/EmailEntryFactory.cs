using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Text;

namespace Inscribe.Email
{
    public class EmailEntryFactory : IEntryFactory<MailMessage>
    {
        public MailMessage Create<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, string message)
        {
            var entryBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(message))
            {
                entryBuilder.AppendLine(message);
            }

            if (exception != null)
            {
                entryBuilder.AppendLine(exception.ToString());
            }

            MailMessage entry = null;

            if (entryBuilder.Length > 0)
            {
                entry = new MailMessage
                {
                    Body = entryBuilder.ToString(),
                    Subject = $"{logLevel} : {loggerName}[{eventId}]",
                };
            }

            return entry;
        }
    }
}
