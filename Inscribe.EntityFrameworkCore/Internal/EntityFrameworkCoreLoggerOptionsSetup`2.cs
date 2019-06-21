using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Inscribe.EntityFrameworkCore.Internal
{
    internal class EntityFrameworkCoreLoggerOptionsSetup<TContext, TEntry> : ConfigureFromConfigurationOptions<EntityFrameworkCoreLoggerOptions>
    where TContext : DbContext
    where TEntry : class
    {
        public EntityFrameworkCoreLoggerOptionsSetup(ILoggerProviderConfiguration<EntityFrameworkCoreLoggerProvider<TContext, TEntry>> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
