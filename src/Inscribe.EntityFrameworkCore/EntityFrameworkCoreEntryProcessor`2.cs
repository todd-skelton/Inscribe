using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class EntityFrameworkCoreEntryProcessor<TContext, TEntry> : EntryProcessorBase<TEntry>
    where TContext : DbContext
    where TEntry : class
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreEntryProcessor(IServiceProvider serviceProvider) : base("Entity Framework Core queue processing thread", 1024)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void WriteEntry(TEntry entry)
        {
            if (entry == null) return;

            // create separate scope for DbContextOptions and DbContext
            using (var scope = _serviceProvider.CreateScope())
            {
                // create separate DbContext for adding log
                // normally we should rely on scope context, but in rare scenarios when DbContext is
                // registered as singleton, we should avoid this.
                using (var context = scope.ServiceProvider.GetService<TContext>())
                {
                    context.Set<TEntry>().Add(entry);
                    context.SaveChanges();
                }
            }
        }
    }
}
