using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Inscribe.Email
{
    public class EmailLogger : LoggerBase<EmailEntryProcessor, MailMessage>
    {
        private DateTime? LastSent;

        internal EmailLogger(string name, IEntryFactory<MailMessage> entryFactory, EmailEntryProcessor entryProcessor, IExternalScopeProvider scopeProvider)
            : base(name, entryFactory, entryProcessor ?? new EmailEntryProcessor(), scopeProvider)
        {
        }

        public TimeSpan? ThrottleTimeout
        {
            get { return _entryProcessor.ThrottleTimeout; }
            set
            {
                _entryProcessor.ThrottleTimeout = value;
            }
        }

        public string ApplicationName
        {
            get { return _entryProcessor.ApplicationName; }
            set
            {
                _entryProcessor.ApplicationName = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public SmtpClient Client
        {
            get { return _entryProcessor.Client; }
            set
            {
                _entryProcessor.Client = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public MailAddress From
        {
            get { return _entryProcessor.From; }
            set
            {
                _entryProcessor.From = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public IEnumerable<MailAddress> To
        {
            get { return _entryProcessor.To; }
            set
            {
                _entryProcessor.To = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public IEnumerable<MailAddress> CC
        {
            get { return _entryProcessor.CC; }
            set
            {
                _entryProcessor.CC = value ?? new List<MailAddress>();
            }
        }

        public IEnumerable<MailAddress> Bcc
        {
            get { return _entryProcessor.Bcc; }
            set
            {
                _entryProcessor.Bcc = value ?? new List<MailAddress>();
            }
        }

        public override void QueueEntry(MailMessage entry)
        {
            if (entry is null)
                return;



            if (LastSent is DateTime && ThrottleTimeout is TimeSpan && DateTime.UtcNow < LastSent.Value.Add(ThrottleTimeout.Value))
                return;

            entry.From = From;
            entry.Subject = $"{ApplicationName} : {entry.Subject}";

            foreach (var recipient in To)
            {
                entry.To.Add(new MailAddress(recipient.Address, recipient.DisplayName));
            }
            foreach (var recipient in CC)
            {
                entry.To.Add(new MailAddress(recipient.Address, recipient.DisplayName));
            }
            foreach (var recipient in Bcc)
            {
                entry.To.Add(new MailAddress(recipient.Address, recipient.DisplayName));
            }

            _entryProcessor.EnqueueEntry(entry);
            LastSent = DateTime.UtcNow;
        }
    }
}
