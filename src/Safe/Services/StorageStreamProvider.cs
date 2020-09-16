using Safe.Core.Services;
using System;
using System.IO;

namespace Safe.Services
{
    /// <summary>
    /// Implements <see cref="IStorageStreamProvider"/> interface.
    /// </summary>
    public class StorageStreamProvider : IStorageStreamProvider
    {
        private readonly IConfigurationService _configurationService;

        public StorageStreamProvider(IConfigurationService configurationService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        /// <inheritdoc/>
        public bool StorageExists
        {
            get
            {
                var configuration = _configurationService.GetConfiguration();

                return File.Exists(configuration.StoragePath);
            }
        }

        /// <inheritdoc/>
        public Stream GetReadStream()
        {
            if (!StorageExists) throw new InvalidOperationException("Storage does not exist");

            var configuration = _configurationService.GetConfiguration();

            return File.OpenRead(configuration.StoragePath);
        }

        /// <inheritdoc/>
        public Stream GetWriteStream()
        {
            var configuration = _configurationService.GetConfiguration();

            return File.OpenWrite(configuration.StoragePath);
        }
    }
}
