using Microsoft.Extensions.Logging;
using System;

namespace Inscribe
{
    /// <summary>
    /// Base class for logger implementations
    /// </summary>
    /// <typeparam name="TEntryProcessor">Type of processor to process threads</typeparam>
    /// <typeparam name="TEntry">Type being used to persist entries</typeparam>
    public abstract class LoggerBase<TEntryProcessor, TEntry> : ILogger, IExternalScopeProviderLogger
        where TEntryProcessor : class, IEntryProcessor<TEntry>
        where TEntry : class
    {
        /// <summary>
        /// Factory to create new entries
        /// </summary>
        protected readonly IEntryFactory<TEntry> _entryFactory;

        /// <summary>
        /// Processor to persist entries
        /// </summary>
        protected readonly TEntryProcessor _entryProcessor;

        /// <summary>
        /// Constuctor for <see cref="LoggerBase{TEntryProcessor, TEntry}"/>
        /// </summary>
        /// <param name="name">Name of the logger</param>
        /// <param name="entryFactory">Factory to create new entries</param>
        /// <param name="entryProcessor">Processor to persist entries</param>
        /// <param name="scopeProvider">Provider for scopes</param>
        protected LoggerBase(string name, IEntryFactory<TEntry> entryFactory, TEntryProcessor entryProcessor, IExternalScopeProvider scopeProvider)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _entryFactory = entryFactory ?? throw new ArgumentNullException(nameof(entryFactory));
            _entryProcessor = entryProcessor ?? throw new ArgumentNullException(nameof(entryProcessor));
            ScopeProvider = scopeProvider;
        }

        /// <summary>
        /// Name of the logger
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Provider for scopes
        /// </summary>
        public virtual IExternalScopeProvider ScopeProvider { get; set; }

        /// <summary>
        /// Method called by the logging framework to persist the log
        /// </summary>
        /// <typeparam name="TState">Type of the logger state</typeparam>
        /// <param name="logLevel">The <see cref="LogLevel"/> being logged</param>
        /// <param name="eventId">The <see cref="EventId"/> being logged</param>
        /// <param name="state">The state being logged</param>
        /// <param name="exception">The <see cref="Exception"/> being logged</param>
        /// <param name="formatter">The formatter used to create the message being logged</param>
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception) ?? "";

            if (!string.IsNullOrWhiteSpace(message) || exception != null)
            {
                ScopeProvider?.ForEachScope<object>((scope, _) => message += Environment.NewLine + scope, null);

                var entry = _entryFactory.Create(Name, logLevel, eventId, state, exception, message);

                QueueEntry(entry);
            }
        }

        /// <summary>
        /// Method that sends the entry created by the <see cref="IEntryFactory{TEntry}"/> to the <see cref="EntryProcessorBase{TEntry}"/>
        /// </summary>
        /// <param name="entry"></param>
        public virtual void QueueEntry(TEntry entry)
        {
            _entryProcessor.EnqueueEntry(entry);
        }

        /// <summary>
        /// Checked if logging is enabled for the current <see cref="LogLevel"/>
        /// </summary>
        /// <param name="logLevel">The <see cref="LogLevel"/> being logged</param>
        /// <returns></returns>
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new scope for the logger
        /// </summary>
        /// <typeparam name="TState">Type of state</typeparam>
        /// <param name="state">State of the new scope</param>
        /// <returns>A new scope that is disposable</returns>
        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state);
    }
}
