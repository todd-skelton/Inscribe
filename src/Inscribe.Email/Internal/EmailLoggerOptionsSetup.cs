using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Inscribe.Email.Internal
{
    internal class EmailLoggerOptionsSetup : ConfigureFromConfigurationOptions<EmailLoggerOptions>
    {
        public EmailLoggerOptionsSetup(ILoggerProviderConfiguration<EmailLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
