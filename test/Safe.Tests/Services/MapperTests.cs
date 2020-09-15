using AutoFixture.Xunit2;
using Safe.Core.Domain;
using Safe.Services;
using System.Text;
using Xunit;

namespace Safe.Tests.Services
{
    public class MapperTests
    {
        private readonly string _salt = "0C9F222E-426A-409C-9026-5D15E49A5E0B";

        [Theory]
        [AutoData]
        public void From_configuration_to_settings(string language, string storagePath)
        {
            // Arrange

            var settings = new Settings();

            var configuration = new Configuration
            {
                Salt = Encoding.ASCII.GetBytes(_salt),
                Language = language,
                StoragePath = storagePath
            };

            var mapper = new Mapper();

            // Act

            mapper.Map(configuration, settings);

            // Assert

            Assert.Equal(language, settings.Language);
            Assert.Equal(storagePath, settings.StoragePath);
            Assert.Equal(_salt, settings.Salt);
        }

        [Theory]
        [AutoData]
        public void From_settings_to_configuration(string language, string storagePath)
        {
            // Arrange

            var settings = new Settings
            {
                Language = language,
                StoragePath = storagePath,
                Salt = _salt
            };

            var configuration = new Configuration();

            var mapper = new Mapper();

            // Act

            mapper.Map(settings, configuration);

            // Assert

            Assert.Equal(language, configuration.Language);
            Assert.Equal(storagePath, configuration.StoragePath);
            Assert.Equal(Encoding.ASCII.GetBytes(_salt), configuration.Salt);
        }
    }
}
