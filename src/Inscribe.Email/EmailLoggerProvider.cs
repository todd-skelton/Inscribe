using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Inscribe.Email
{
    [ProviderAlias("Email")]
    public class EmailLoggerProvider : LoggerProviderBase<EmailLogger, EmailLoggerOptions, EmailEntryProcessor, MailMessage>
    {
        public EmailLoggerProvider(IOptionsMonitor<EmailLoggerOptions> options, IEntryFactory<MailMessage> entryFactory) : base(options, entryFactory, new EmailEntryProcessor())
        {
            CreateOrUpdateClient();
        }

        protected override void ReloadLoggerOptions(EmailLoggerOptions options)
        {
            base.ReloadLoggerOptions(options);

            CreateOrUpdateClient();
        }

        private void CreateOrUpdateClient()
        {
            if (_options.Smtp == null || string.IsNullOrWhiteSpace(_options.Smtp.Host))
                _entryProcessor.Client = new SmtpClient();
            else
                _entryProcessor.Client = new SmtpClient(_options.Smtp.Host);

            if (_options.Smtp?.UseDefaultCredentials == false)
                _entryProcessor.Client.UseDefaultCredentials = false;

            if (_options.Smtp?.Credentials != null)
            {
                _entryProcessor.Client.Credentials = _options.Smtp.Credentials.Domain == null ? new NetworkCredential(_options.Smtp.Credentials.Username, _options.Smtp.Credentials.Password) : new NetworkCredential(_options.Smtp.Credentials.Username, _options.Smtp.Credentials.Password, _options.Smtp.Credentials.Domain);
            }

            _entryProcessor.ApplicationName = _options.ApplicationName ?? throw new ArgumentNullException(nameof(_options.ApplicationName));
            _entryProcessor.From = new MailAddress(_options.From.Address, _options.From.DisplayName) ?? throw new ArgumentNullException(nameof(_options.From));
            _entryProcessor.To = _options.To?.Select(e => new MailAddress(e.Address, e.DisplayName)) ?? throw new ArgumentNullException(nameof(_options.To));
            _entryProcessor.CC = _options.CC?.Select(e => new MailAddress(e.Address, e.DisplayName)) ?? new List<MailAddress>();
            _entryProcessor.Bcc = _options.Bcc?.Select(e => new MailAddress(e.Address, e.DisplayName)) ?? new List<MailAddress>();
            _entryProcessor.ThrottleTimeout = _options.ThrottleTimeout;
        }

        protected override EmailLogger CreateLoggerImplementation(string name)
        {
            return new EmailLogger(name, _entryFactory, _entryProcessor, GetScopeProvider());
        }
    }
}
