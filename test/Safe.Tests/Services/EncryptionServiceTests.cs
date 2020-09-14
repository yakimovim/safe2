using AutoFixture.Xunit2;
using Moq;
using Safe.Core.Domain;
using Safe.Core.Services;
using System.IO;
using System.Text;
using Xunit;

namespace Safe.Tests.Services
{
    public class EncryptionServiceTests
    {
        private readonly IConfigurationService _configurationService;

        public EncryptionServiceTests()
        {
            var salt = Encoding.ASCII.GetBytes("014659A4-76CA-43D7-8A53-B35E218F6CDD");

            var configurationMoq = new Mock<IConfiguration>();
            configurationMoq
                .Setup(config => config.Salt)
                .Returns(salt);

            var configurationServiceMoq = new Mock<IConfigurationService>();
            configurationServiceMoq
                .Setup(service => service.GetConfiguration())
                .Returns(configurationMoq.Object);

            _configurationService = configurationServiceMoq.Object;
        }

        [Theory]
        [AutoData]
        public void Encryption_decryption_test(string password, string initialText)
        {
            // Arrange

            var service = new EncryptionService(_configurationService);

            // Act

            var stream = new MemoryStream();

            service.Encrypt(password, initialText, stream);

            var buffer = stream.ToArray();

            stream = new MemoryStream(buffer);

            var decryptedText = service.Decrypt<string>(password, stream);

            // Assert

            Assert.Equal(initialText, decryptedText);
        }
    }
}
