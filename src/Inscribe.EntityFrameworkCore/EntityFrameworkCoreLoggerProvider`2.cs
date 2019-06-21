using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Inscribe.EntityFrameworkCore
{
    [ProviderAlias("EntityFrameworkCore")]
    public class EntityFrameworkCoreLoggerProvider<TContext, TEntry> : LoggerProviderBase<EntityFrameworkCoreLogger<TContext, TEntry>, EntityFrameworkCoreLoggerOptions, EntityFrameworkCoreEntryProcessor<TContext, TEntry>, TEntry>
    where TContext : DbContext
    where TEntry : class
    {
        protected readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreLoggerProvider(IServiceProvider serviceProvider, IOptionsMonitor<EntityFrameworkCoreLoggerOptions> options, IEntryFactory<TEntry> entryFactory) : base(options, entryFactory, new EntityFrameworkCoreEntryProcessor<TContext, TEntry>(serviceProvider))
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override EntityFrameworkCoreLogger<TContext, TEntry> CreateLoggerImplementation(string name)
        {
            return new EntityFrameworkCoreLogger<TContext, TEntry>(_serviceProvider, name, _entryFactory, _entryProcessor, GetScopeProvider());
        }
    }
}
