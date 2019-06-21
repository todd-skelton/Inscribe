using Inscribe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Extensions for adding logging providers using <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static class LoggerBuilderExtensions
    {
        /// <summary>
        /// Configures a logger using options from <see cref="IConfiguration"/>.
        /// </summary>
        /// <typeparam name="TLoggerProvider">The concrete implementation type of <see cref="ILoggerProvider"/>.</typeparam>
        /// <typeparam name="TLogger">The concrete implementation type of <see cref="ILogger"/> and <see cref="IExternalScopeProviderLogger"/>.</typeparam>
        /// <typeparam name="TLoggerOptions">The concrete implementation type of <see cref="ILoggerOptions"/>.</typeparam>
        /// <typeparam name="TEntryProcessor">The concrete implementation type of <see cref="IEntryProcessor{TEntry}"/>.</typeparam>
        /// <typeparam name="TEntry">The type of entry being used to create the log entry using <see cref="IEntryFactory{TEntry}"/></typeparam>
        /// <typeparam name="TLoggerOptionsSetup">The concrete implementation type <see cref="IConfigureOptions{TLoggerOptions}"/></typeparam>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        public static ILoggingBuilder AddLoggerBase<TLoggerProvider, TLogger, TLoggerOptions, TEntryProcessor, TEntry, TLoggerOptionsSetup>(
            this ILoggingBuilder builder
        )
            where TLoggerProvider : LoggerProviderBase<TLogger, TLoggerOptions, TEntryProcessor, TEntry>
            where TLogger : ILogger, IExternalScopeProviderLogger
            where TLoggerOptions : class, ILoggerOptions
            where TEntryProcessor : class, IEntryProcessor<TEntry>
            where TEntry : class
            where TLoggerOptionsSetup : class, IConfigureOptions<TLoggerOptions>
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TLoggerOptions>, TLoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TLoggerOptions>, LoggerProviderOptionsChangeTokenSource<TLoggerOptions, TLoggerProvider>>());
            return builder;
        }

        /// <summary>
        /// Configures a logger using options from an <see cref="Action"/>
        /// </summary>
        /// <typeparam name="TLoggerProvider">The concrete implementation type of <see cref="ILoggerProvider"/>.</typeparam>
        /// <typeparam name="TLogger">The concrete implementation type of <see cref="ILogger"/> and <see cref="IExternalScopeProviderLogger"/>.</typeparam>
        /// <typeparam name="TLoggerOptions">The concrete implementation type of <see cref="ILoggerOptions"/>.</typeparam>
        /// <typeparam name="TEntryProcessor">The concrete implementation type of <see cref="IEntryProcessor{TEntry}"/>.</typeparam>
        /// <typeparam name="TEntry">The type of entry being used to create the log entry using <see cref="IEntryFactory{TEntry}"/></typeparam>
        /// <typeparam name="TLoggerOptionsSetup">The concrete implementation type <see cref="IConfigureOptions{TLoggerOptions}"/></typeparam>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configure">The <see cref="Action"/> to create the configuration</param>
        /// <returns></returns>
        public static ILoggingBuilder AddLoggerBase<TLoggerProvider, TLogger, TLoggerOptions, TEntryProcessor, TEntry, TLoggerOptionsSetup>(
            this ILoggingBuilder builder,
            Action<TLoggerOptions> configure
        )
            where TLoggerProvider : LoggerProviderBase<TLogger, TLoggerOptions, TEntryProcessor, TEntry>
            where TLogger : ILogger, IExternalScopeProviderLogger
            where TLoggerOptions : class, ILoggerOptions
            where TEntryProcessor : class, IEntryProcessor<TEntry>
            where TEntry : class
            where TLoggerOptionsSetup : class, IConfigureOptions<TLoggerOptions>
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddLoggerBase<TLoggerProvider, TLogger, TLoggerOptions, TEntryProcessor, TEntry, TLoggerOptionsSetup>();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
