using Safe.Core.Domain;
using System;
using System.Diagnostics;

namespace Safe.Core.Services
{
    /// <summary>
    /// Represents storage.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Checks if the storage exists.
        /// </summary>
        bool Exists { get; }
        /// <summary>
        /// Checks if the a user is logged into the storage.
        /// </summary>
        bool LoggedIn { get; }
        /// <summary>
        /// Creates new storage.
        /// </summary>
        /// <param name="password">Password.</param>
        void Create(Password password);

        /// <summary>
        /// Logs into the storage.
        /// </summary>
        /// <param name="password">Password.</param>
        bool Login(Password password);
        /// <summary>
        /// Logs out of the storage.
        /// </summary>
        void Logout();
        /// <summary>
        /// Changes password of the storage.
        /// </summary>
        /// <param name="oldPassword">Old password.</param>
        /// <param name="newPassword">New password.</param>
        void ChangePassword(Password oldPassword, Password newPassword);

        /// <summary>
        /// Reads content of the storage.
        /// </summary>
        Container Read();
        /// <summary>
        /// Saves content of the storage.
        /// </summary>
        /// <param name="container">Content of the storage.</param>
        void Save(Container container);
    }

    /// <summary>
    /// Implements <see cref="IStorage"/> interface.
    /// </summary>
    public sealed class Storage : IStorage
    {
        private readonly IStorageStreamProvider _storageStreamProvider;
        private readonly IEncryptionService _encryptionService;

        private Password _password = null;

        [DebuggerStepThrough]
        public Storage(
            IStorageStreamProvider storageStreamProvider,
            IEncryptionService encryptionService
            )
        {
            _storageStreamProvider = storageStreamProvider ?? throw new ArgumentNullException(nameof(storageStreamProvider));
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        public bool LoggedIn => _password != null;

        public bool Exists => _storageStreamProvider.StorageExists;

        public void ChangePassword(Password oldPassword, Password newPassword)
        {
            if (oldPassword is null)
            {
                throw new ArgumentNullException(nameof(oldPassword));
            }

            if (newPassword is null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            if (!Exists) throw new InvalidOperationException("Storage does not exist.");
            if (!LoggedIn) throw new InvalidOperationException("You should log in before changing the password.");

            if (!_password.Equals(oldPassword)) throw new ArgumentException("Incorrect old password", nameof(oldPassword));

            var container = Read();

            _password = newPassword;

            Save(container);
        }

        public void Create(Password password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (Exists) throw new InvalidOperationException("Storage already exists");

            var container = new Container();

            InternalSave(container, password);
        }

        public bool Login(Password password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!Exists) throw new InvalidOperationException("Storage does not exist.");

            try
            {
                _password = password;

                Read();

                return true;
            }
            catch
            {
                _password = null;

                return false;
            }
        }

        public void Logout()
        {
            if (!LoggedIn) throw new InvalidOperationException("You are not logged in");

            _password = null;
        }

        public Container Read()
        {
            if (!Exists) throw new InvalidOperationException("Storage does not exist.");
            if (!LoggedIn) throw new InvalidOperationException("You are not logged in");

            using (var readStream = _storageStreamProvider.GetReadStream())
            {
                return _encryptionService.Decrypt<Container>(_password, readStream);
            }
        }

        public void Save(Container container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (!Exists) throw new InvalidOperationException("Storage does not exist.");
            if (!LoggedIn) throw new InvalidOperationException("You are not logged in");

            InternalSave(container, _password);
        }

        private void InternalSave(Container container, Password password)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (var writeStream = _storageStreamProvider.GetWriteStream())
            {
                _encryptionService.Encrypt(password, container, writeStream);
            }
        }
    }
}
