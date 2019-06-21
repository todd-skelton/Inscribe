using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class EntityFrameworkCoreLogger<TContext> : EntityFrameworkCoreLogger<TContext, Entry>
    where TContext : DbContext
    {
        public EntityFrameworkCoreLogger(
            IServiceProvider serviceProvider,
            string name,
            IEntryFactory<Entry> entryFactory,
            EntityFrameworkCoreEntryProcessor<TContext> entryProcessor,
            IExternalScopeProvider scopeProvider
        )
            : base(serviceProvider, name, entryFactory, entryProcessor, scopeProvider)
        {
        }
    }
}
