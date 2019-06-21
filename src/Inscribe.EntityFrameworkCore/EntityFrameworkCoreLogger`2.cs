using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class EntityFrameworkCoreLogger<TContext, TEntry> : LoggerBase<EntityFrameworkCoreEntryProcessor<TContext, TEntry>, TEntry>
    where TContext : DbContext
    where TEntry : class
    {
        internal EntityFrameworkCoreLogger(
            IServiceProvider serviceProvider,
            string name,
            IEntryFactory<TEntry> entryFactory,
            EntityFrameworkCoreEntryProcessor<TContext, TEntry> entryProcessor,
            IExternalScopeProvider scopeProvider
        )
            : base(name, entryFactory, entryProcessor ?? new EntityFrameworkCoreEntryProcessor<TContext, TEntry>(serviceProvider), scopeProvider)
        {
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            if (Name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return base.IsEnabled(logLevel);
        }
    }
}
