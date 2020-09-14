using Safe.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
