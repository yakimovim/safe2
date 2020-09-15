using Moq;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Tests.Domain.Utilities;
using System;
using System.Text;
using Xunit;

namespace Safe.Tests.Services
{
    public class StorageTests
    {
        private readonly Password _password;
        private readonly FakeStorageStreamProvider _storageStreamProvider;
        private readonly Storage _storage;

        public StorageTests()
        {
            _password = new Password("E8AED495-27E4-4E62-8C6A-F94B811AD910");
            _storageStreamProvider = new FakeStorageStreamProvider();

            var configuration = new Configuration
            {
                Salt = Encoding.ASCII.GetBytes("8AD919F7-96E2-49CF-9EE4-FFC11E9B8554")
            };
            var configurationServiceMock = new Mock<IConfigurationService>();
            configurationServiceMock.Setup(service => service.GetConfiguration()).Returns(configuration);

            var encryptionService = new EncryptionService(configurationServiceMock.Object);

            _storage = new Storage(_storageStreamProvider, encryptionService); 
        }

        [Fact]
        public void Cant_login_if_storage_does_not_exist()
        {
            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Login(_password));
        }
    }
}
