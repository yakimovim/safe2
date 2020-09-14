using Safe.Core.Domain;
using Safe.Core.Services;
using System;

namespace Safe.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IMapper _mapper;

        public ConfigurationService(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IConfiguration GetConfiguration()
        {
            var configuration = new Configuration();

            _mapper.Map(Settings.Default, configuration);

            return configuration;
        }

        public void SaveConfiguration(IConfiguration configuration)
        {
            _mapper.Map(configuration, Settings.Default);

            Settings.Default.Save();
        }
    }
}
