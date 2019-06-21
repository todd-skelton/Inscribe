using Inscribe;
using Inscribe.Email;
using Inscribe.Email.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Mail;

namespace Microsoft.Extensions.Logging
{
    public static class EmailLoggingBuilderExtensions
    {
        public static ILoggingBuilder AddEmail(this ILoggingBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEmail<EmailEntryFactory>();
        }

        public static ILoggingBuilder AddEmail<TEntryFactory>(this ILoggingBuilder builder)
            where TEntryFactory : class, IEntryFactory<MailMessage>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IEntryFactory<MailMessage>, TEntryFactory>());
            return builder.AddLoggerBase<EmailLoggerProvider, EmailLogger, EmailLoggerOptions, EmailEntryProcessor, MailMessage, EmailLoggerOptionsSetup>();
        }

        public static ILoggingBuilder AddEmail(this ILoggingBuilder builder, Action<EmailLoggerOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEmail<EmailEntryFactory>(configure);
        }

        public static ILoggingBuilder AddEmail<TEntryFactory>(this ILoggingBuilder builder, Action<EmailLoggerOptions> configure)
            where TEntryFactory : class, IEntryFactory<MailMessage>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IEntryFactory<MailMessage>, TEntryFactory>());
            builder.AddLoggerBase<EmailLoggerProvider, EmailLogger, EmailLoggerOptions, EmailEntryProcessor, MailMessage, EmailLoggerOptionsSetup>(configure);
            return builder;
        }
    }
}