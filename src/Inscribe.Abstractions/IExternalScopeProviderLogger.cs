using Microsoft.Extensions.Logging;

namespace Inscribe
{
    /// <summary>
    /// Interface for loggers that use an <see cref="IExternalScopeProvider"/>
    /// </summary>
    public interface IExternalScopeProviderLogger
    {
        /// <summary>
        /// The <see cref="IExternalScopeProvider"/>
        /// </summary>
        IExternalScopeProvider ScopeProvider { get; set; }
    }
}
