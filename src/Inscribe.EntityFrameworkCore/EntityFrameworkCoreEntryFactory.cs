using Microsoft.Extensions.Logging;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class EntityFrameworkCoreEntryFactory : IEntryFactory<Entry>
    {
        /// <inheritdoc/>
        public Entry Create<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, string message)
        {
            return new Entry()
            {
                Id = Guid.NewGuid(),
                Level = logLevel,
                Name = loggerName ?? "",
                EventId = eventId.Id,
                Message = message ?? "",
                Exception = exception?.ToString() ?? "",
                Timestamp = DateTimeOffset.Now
            };
        }
    }
}
