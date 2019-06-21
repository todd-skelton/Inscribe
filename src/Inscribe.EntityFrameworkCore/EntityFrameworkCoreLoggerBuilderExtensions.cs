using Inscribe;
using Inscribe.EntityFrameworkCore;
using Inscribe.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// <see cref="ILoggingBuilder"/> extensions to configure logging for Entity Framework Core
    /// </summary>
    public static class EntityFrameworkCoreLoggerBuilderExtensions
    {
        /// <summary>
        /// Configures a <see cref="DbContext"/> to use for logging
        /// </summary>
        /// <typeparam name="TContext">The <see cref="DbContext"/></typeparam>
        /// <param name="builder">The <see cref="ILoggingBuilder"/></param>
        /// <param name="optionsAction">The <see cref="DbContextOptions"/> to use</param>
        /// <returns>The <see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddEntityFrameworkCore<TContext>(this ILoggingBuilder builder, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEntityFrameworkCore<TContext, EntityFrameworkCoreEntryFactory>(optionsAction);
        }

        public static ILoggingBuilder AddEntityFrameworkCore<TContext, TEntryFactory>(this ILoggingBuilder builder, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            where TEntryFactory : class, IEntryFactory<Entry>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEntityFrameworkCore<TContext, TEntryFactory, Entry>(optionsAction);
        }

        public static ILoggingBuilder AddEntityFrameworkCore<TContext, TEntryFactory, TEntry>(this ILoggingBuilder builder, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            where TEntryFactory : class, IEntryFactory<TEntry>
            where TEntry : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IEntryFactory<TEntry>, TEntryFactory>());
            builder.Services.AddDbContext<TContext>(optionsAction);
            builder.AddLoggerBase<EntityFrameworkCoreLoggerProvider<TContext, TEntry>, EntityFrameworkCoreLogger<TContext, TEntry>, EntityFrameworkCoreLoggerOptions, EntityFrameworkCoreEntryProcessor<TContext, TEntry>, TEntry, EntityFrameworkCoreLoggerOptionsSetup<TContext, TEntry>>();

            return builder;
        }

        public static ILoggingBuilder AddEntityFrameworkCore<TContext>(this ILoggingBuilder builder, Action<EntityFrameworkCoreLoggerOptions> configure, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEntityFrameworkCore<TContext, EntityFrameworkCoreEntryFactory>(configure, optionsAction);
        }

        public static ILoggingBuilder AddEntityFrameworkCore<TContext, TEntryFactory>(this ILoggingBuilder builder, Action<EntityFrameworkCoreLoggerOptions> configure, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            where TEntryFactory : class, IEntryFactory<Entry>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.AddEntityFrameworkCore<TContext, TEntryFactory, Entry>(configure, optionsAction);
        }

        public static ILoggingBuilder AddEntityFrameworkCore<TContext, TEntryFactory, TEntry>(this ILoggingBuilder builder, Action<EntityFrameworkCoreLoggerOptions> configure, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            where TEntryFactory : class, IEntryFactory<TEntry>
            where TEntry : class
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IEntryFactory<TEntry>, TEntryFactory>());
            builder.Services.AddDbContext<TContext>();
            builder.AddLoggerBase<EntityFrameworkCoreLoggerProvider<TContext, TEntry>, EntityFrameworkCoreLogger<TContext, TEntry>, EntityFrameworkCoreLoggerOptions, EntityFrameworkCoreEntryProcessor<TContext, TEntry>, TEntry, EntityFrameworkCoreLoggerOptionsSetup<TContext, TEntry>>(configure);

            return builder;
        }
    }
}