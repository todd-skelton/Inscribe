namespace Inscribe
{
    /// <summary>
    /// Options used by the logger
    /// </summary>
    public interface ILoggerOptions
    {
        /// <summary>
        /// Whether or not to use scopes
        /// </summary>
        bool IncludeScopes { get; }
    }
}
