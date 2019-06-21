using Microsoft.Extensions.Logging;
using System;

namespace Inscribe
{
    /// <summary>
    /// Interface for a factory to create new entries
    /// </summary>
    /// <typeparam name="TEntry">The type of entry being created</typeparam>
    public interface IEntryFactory<TEntry>
        where TEntry : class
    {
        /// <summary>
        /// Takes information for the logger and creates an entry
        /// </summary>
        /// <typeparam name="TState">Type of state being used</typeparam>
        /// <param name="loggerName">Name of the logger</param>
        /// <param name="logLevel"><see cref="LogLevel"/> being logged</param>
        /// <param name="eventId"><see cref="EventId"/> being logged</param>
        /// <param name="state">State of the logger</param>
        /// <param name="exception"><see cref="Exception"/> being logged</param>
        /// <param name="message">Message being logged</param>
        /// <returns></returns>
        TEntry Create<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, string message);
    }
}
