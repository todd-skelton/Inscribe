using System;

namespace Inscribe
{
    /// <summary>
    /// Interface used to create an implementation for processing log entries on a separate thread
    /// </summary>
    /// <typeparam name="TEntry">Type used to persist the entry</typeparam>
    public interface IEntryProcessor<TEntry> : IDisposable
        where TEntry : class
    {
        /// <summary>
        /// Adds the entry to the processor queue
        /// </summary>
        /// <param name="entry">The entry to add</param>
        void EnqueueEntry(TEntry entry);
    }
}
