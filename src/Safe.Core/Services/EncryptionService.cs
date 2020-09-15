using Safe.Core.Domain;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Safe.Core.Services
{
    /// <summary>
    /// Represents service to encrypt and decrypt objects.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts <paramref name="valueToEncrypt"/> object.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <param name="valueToEncrypt">Value to encrypt.</param>
        /// <param name="targetStream">Stream to encrypt to.</param>
        void Encrypt(Password password, object valueToEncrypt, Stream targetStream);
        /// <summary>
        /// Decrypts object of type <typeparamref name="T"/> from the <paramref name="sourceStream"/> stream.
        /// </summary>
        /// <typeparam name="T">Type of object to decrypt.</typeparam>
        /// <param name="password">Password.</param>
        /// <param name="sourceStream">Stream to decrypt from.</param>
        /// <returns></returns>
        T Decrypt<T>(Password password, Stream sourceStream);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly IConfigurationService _configurationService;

        [DebuggerStepThrough]
        public EncryptionService(IConfigurationService configurationService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public void Encrypt(Password password, object valueToEncrypt, Stream targetStream)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (targetStream is null)
                throw new ArgumentNullException(nameof(targetStream));

            SymmetricAlgorithm encryptionAlgorithm = GetEncryptionAlgorithm(password);
            encryptionAlgorithm.GenerateIV();

            targetStream.Write(encryptionAlgorithm.IV, 0, encryptionAlgorithm.IV.Length);

            using (CryptoStream cStrm = new CryptoStream(
                targetStream,
                encryptionAlgorithm.CreateEncryptor(),
                CryptoStreamMode.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(cStrm, valueToEncrypt);
            }
        }

        public T Decrypt<T>(Password password, Stream sourceStream)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (sourceStream is null)
                throw new ArgumentNullException(nameof(sourceStream));

            SymmetricAlgorithm encryptionAlgorithm = GetEncryptionAlgorithm(password);

            var ivBuffer = new byte[encryptionAlgorithm.BlockSize / 8];
            sourceStream.Read(ivBuffer, 0, ivBuffer.Length);

            encryptionAlgorithm.IV = ivBuffer;

            using (CryptoStream cStrm = new CryptoStream(
                sourceStream, 
                encryptionAlgorithm.CreateDecryptor(), 
                CryptoStreamMode.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(cStrm);
            }
        }

        private SymmetricAlgorithm GetEncryptionAlgorithm(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password can't be null or empty.", nameof(password));

            SymmetricAlgorithm algorithm = Aes.Create();
            algorithm.Mode = CipherMode.CBC;
            algorithm.BlockSize = 128;
            algorithm.KeySize = 256;

            var configuration = _configurationService.GetConfiguration();

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(
                password,
                configuration.Salt
            );

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);

            return algorithm;

        }
    }
}
