using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Inscribe.EntityFrameworkCore
{
    [ProviderAlias("EntityFrameworkCore")]
    public class EntityFrameworkCoreLoggerProvider<TContext> : EntityFrameworkCoreLoggerProvider<TContext, Entry>
    where TContext : DbContext
    {
        public EntityFrameworkCoreLoggerProvider(IServiceProvider serviceProvider, IOptionsMonitor<EntityFrameworkCoreLoggerOptions> options)
            : this(serviceProvider, options, new EntityFrameworkCoreEntryFactory())
        {
        }

        public EntityFrameworkCoreLoggerProvider(IServiceProvider serviceProvider, IOptionsMonitor<EntityFrameworkCoreLoggerOptions> options, IEntryFactory<Entry> entryFactory)
            : base(serviceProvider, options, entryFactory)
        {
        }
    }
}
