using AutoFixture.Xunit2;
using Moq;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Tests.Services.Utilities;
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

        [Fact]
        public void Successful_login()
        {
            // Arrange

            CreateStorage();

            // Assert

            Assert.False(_storage.LoggedIn);

            Assert.True(_storage.Login(_password));
            
            Assert.True(_storage.LoggedIn);
        }

        [Fact]
        public void Unsuccessful_login()
        {
            // Arrange

            CreateStorage();

            // Assert

            Assert.False(_storage.LoggedIn);

            Assert.False(_storage.Login(new Password("706CFD1B-505C-4A84-ABB1-E40F7AD79BB9")));

            Assert.False(_storage.LoggedIn);
        }

        [Fact]
        public void Cant_log_out_if_not_logged_in()
        {
            // Arrange

            CreateStorage();

            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Logout());
        }

        [Fact]
        public void Successful_logout()
        {
            // Arrange

            CreateStorage();

            _storage.Login(_password);

            // Assert

            Assert.True(_storage.LoggedIn);

            _storage.Logout();

            Assert.False(_storage.LoggedIn);
        }

        [Fact]
        public void Cant_create_storage_if_it_exists()
        {
            // Arrange

            _storageStreamProvider.StorageExists = true;

            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Create(_password));
        }

        [Fact]
        public void Cant_read_storage_if_storage_does_not_exist()
        {
            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Read());
        }

        [Fact]
        public void Cant_read_storage_if_is_not_logged_in()
        {
            // Arrange

            CreateStorage();

            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Read());
        }

        [Fact]
        public void Cant_write_to_storage_if_storage_does_not_exist()
        {
            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Save(new Container()));
        }

        [Fact]
        public void Cant_write_to_storage_if_is_not_logged_in()
        {
            // Arrange

            CreateStorage();

            // Assert

            Assert.Throws<InvalidOperationException>(() => _storage.Save(new Container()));
        }

        [Theory]
        [AutoData]
        public void Read_what_was_saved(string title, string description)
        {
            // Arrange

            CreateStorage();

            _storage.Login(_password);

            var item = new Item
            {
                Description = description,
                Title = title
            };

            var container = new Container();
            container.Items.Add(item);

            // Act

            _storage.Save(container);

            container = _storage.Read();

            // Assert

            Assert.NotNull(container);
            item = Assert.Single(container.Items);
            Assert.Equal(title, item.Title);
            Assert.Equal(description, item.Description);
        }

        private void CreateStorage()
        {
            _storageStreamProvider.StorageExists = false;

            _storage.Create(_password);

            _storageStreamProvider.StorageExists = true;
        }
    }
}
