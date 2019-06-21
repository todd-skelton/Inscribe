using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Inscribe
{
    /// <summary>
    /// Abstract class used to create an implementation for processing log entries on a separate thread
    /// </summary>
    /// <typeparam name="TEntry">Type used to persist the entry</typeparam>
    public abstract class EntryProcessorBase<TEntry> : IEntryProcessor<TEntry>
        where TEntry : class
    {
        /// <summary>
        /// The maximum number of entries that should be queued
        /// </summary>
        protected readonly int _maxQueuedEntries;

        /// <summary>
        /// A thread safe queue for entries that need to be processed
        /// </summary>
        protected readonly BlockingCollection<TEntry> _entryQueue;

        /// <summary>
        /// The thread to process the entires on
        /// </summary>
        protected readonly Thread _outputThread;

        /// <summary>
        /// Constructor for base class
        /// </summary>
        /// <param name="name">The name of the thread</param>
        /// <param name="maxQueuedEntries">The maximum number of entries that should be queued</param>
        protected EntryProcessorBase(string name, int maxQueuedEntries)
        {
            _maxQueuedEntries = maxQueuedEntries;
            _entryQueue = new BlockingCollection<TEntry>(_maxQueuedEntries);

            // Start Console message queue processor
            _outputThread = new Thread(ProcessEntryQueue)
            {
                IsBackground = true,
                Name = name
            };
            _outputThread.Start();
        }

        /// <summary>
        /// Enqueue an entry
        /// </summary>
        /// <param name="entry"></param>
        public virtual void EnqueueEntry(TEntry entry)
        {
            if (!_entryQueue.IsAddingCompleted)
            {
                try
                {
                    _entryQueue.Add(entry);
                    return;
                }
                catch (InvalidOperationException) { }
            }
            WriteEntry(entry);
        }

        /// <summary>
        /// Implementation of persisting the entry
        /// </summary>
        /// <param name="entry"></param>
        protected abstract void WriteEntry(TEntry entry);

        /// <summary>
        /// Process the entry queue
        /// </summary>
        protected virtual void ProcessEntryQueue()
        {
            try
            {
                foreach (var entry in _entryQueue.GetConsumingEnumerable())
                {
                    WriteEntry(entry);
                }
            }
            catch
            {
                try
                {
                    _entryQueue.CompleteAdding();
                }
                catch { }
            }
        }

        /// <summary>
        /// Disposes this class and the thread
        /// </summary>
        public virtual void Dispose()
        {
            _entryQueue.CompleteAdding();

            try
            {
                _outputThread.Join(1500);
            }
            catch (ThreadStateException) { }
        }
    }
}
