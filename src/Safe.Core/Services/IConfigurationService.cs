using Safe.Core.Domain;

namespace Safe.Core.Services
{
    /// <summary>
    /// Service for work with configuration.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets configuration.
        /// </summary>
        IConfiguration GetConfiguration();
        /// <summary>
        /// Saves configuration.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        void SaveConfiguration(IConfiguration configuration);
    }
}
