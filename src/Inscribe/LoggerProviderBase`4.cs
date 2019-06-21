using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace Inscribe
{
    /// <summary>
    /// Base class for implementing a new <see cref="ILoggerProvider"/>
    /// </summary>
    /// <typeparam name="TLogger">Type of <see cref="ILogger"/> being provided</typeparam>
    /// <typeparam name="TLoggerOptions">Type of <see cref="ILoggerOptions"/> to use</typeparam>
    /// <typeparam name="TEntryProcessor">Type of <see cref="IEntryProcessor{TEntry}"/> to process entries</typeparam>
    /// <typeparam name="TEntry">Type of entry used to persist</typeparam>
    public abstract class LoggerProviderBase<TLogger, TLoggerOptions, TEntryProcessor, TEntry> : ILoggerProvider, ISupportExternalScope
        where TLogger : ILogger, IExternalScopeProviderLogger
        where TLoggerOptions : class, ILoggerOptions
        where TEntryProcessor : class, IEntryProcessor<TEntry>
        where TEntry : class
    {
        /// <summary>
        /// <see cref="ConcurrentDictionary{TKey, TValue}"/> of loggers that have been created
        /// </summary>
        protected readonly ConcurrentDictionary<string, TLogger> _loggers = new ConcurrentDictionary<string, TLogger>();
        /// <summary>
        /// <see cref="IEntryFactory{TEntry}"/> to create new entries
        /// </summary>
        protected readonly IEntryFactory<TEntry> _entryFactory;
        /// <summary>
        /// <see cref="IEntryProcessor{TEntry}"/> to persist entries
        /// </summary>
        protected readonly TEntryProcessor _entryProcessor;
        /// <summary>
        /// <see cref="ILoggerOptions"/> to configure the loggers
        /// </summary>
        protected TLoggerOptions _options;
        /// <summary>
        /// Token used to trigger a change to the options and update the loggers
        /// </summary>
        protected IDisposable _optionsReloadToken;
        /// <summary>
        /// <see cref="IExternalScopeProvider"/> for creating scopes
        /// </summary>
        protected IExternalScopeProvider _scopeProvider;

        /// <summary>
        /// Constructor for creating a <see cref="LoggerProviderBase{TLogger, TLoggerOptions, TEntryProcessor, TEntry}"/>
        /// </summary>
        /// <param name="options"><see cref="IOptions{TOptions}"/> to configure the loggers</param>
        /// <param name="entryFactory"><see cref="IEntryFactory{TEntry}"/> to create entries</param>
        /// <param name="entryProcessor"><see cref="IEntryProcessor{TEntry}"/> to persist entries</param>
        protected LoggerProviderBase(IOptionsMonitor<TLoggerOptions> options, IEntryFactory<TEntry> entryFactory, TEntryProcessor entryProcessor) : this(entryFactory, entryProcessor)
        {
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        private LoggerProviderBase(IEntryFactory<TEntry> entryFactory, TEntryProcessor entryProcessor)
        {
            _entryFactory = entryFactory ?? throw new ArgumentNullException(nameof(entryFactory));
            _entryProcessor = entryProcessor ?? throw new ArgumentNullException(nameof(entryProcessor));
        }

        /// <summary>
        /// Reloads the options when a change is triggered
        /// </summary>
        /// <param name="options"></param>
        protected virtual void ReloadLoggerOptions(TLoggerOptions options)
        {
            _options = options;

            var scopeProvider = GetScopeProvider();
            foreach (var logger in _loggers.Values)
            {
                logger.ScopeProvider = scopeProvider;
            }
        }

        /// <summary>
        /// Creates a new logger and adds it to <see cref="_loggers"/>
        /// </summary>
        /// <param name="name">Name of the logger being created</param>
        /// <returns>A new logger</returns>
        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name, CreateLoggerImplementation);
        }

        /// <summary>
        /// Implementation for creating a logger
        /// </summary>
        /// <param name="name">Name of the logger</param>
        /// <returns>A new logger</returns>
        protected abstract TLogger CreateLoggerImplementation(string name);

        /// <summary>
        /// Gets the <see cref="IExternalScopeProvider"/> for the logger
        /// </summary>
        /// <returns></returns>
        protected virtual IExternalScopeProvider GetScopeProvider()
        {
            if (_options.IncludeScopes && _scopeProvider == null)
            {
                _scopeProvider = new LoggerExternalScopeProvider();
            }
            return _options.IncludeScopes ? _scopeProvider : null;
        }

        /// <summary>
        /// Disposes the <see cref="_entryProcessor"/> and <see cref="_optionsReloadToken"/>
        /// </summary>
        public virtual void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _entryProcessor.Dispose();
        }

        /// <summary>
        /// Sets the <see cref="_scopeProvider"/> for the provider
        /// </summary>
        /// <param name="scopeProvider">The <see cref="IExternalScopeProvider"/> to be set</param>
        public virtual void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
}
