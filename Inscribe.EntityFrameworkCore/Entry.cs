using Microsoft.Extensions.Logging;
using System;

namespace Inscribe.EntityFrameworkCore
{
    public class Entry
    {
        /// <summary>
        /// Gets or sets the primary key for this log.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public virtual LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the event Id.
        /// </summary>
        public virtual int EventId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception
        /// </summary>
        public virtual string Exception { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        public virtual DateTimeOffset Timestamp { get; set; }
    }
}
