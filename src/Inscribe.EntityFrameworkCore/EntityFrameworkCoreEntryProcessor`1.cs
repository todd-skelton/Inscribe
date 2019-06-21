using Microsoft.EntityFrameworkCore;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class EntityFrameworkCoreEntryProcessor<TContext> : EntityFrameworkCoreEntryProcessor<TContext, Entry>
    where TContext : DbContext
    {
        public EntityFrameworkCoreEntryProcessor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
