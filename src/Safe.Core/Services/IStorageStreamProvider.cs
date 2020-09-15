using System.IO;

namespace Safe.Core.Services
{
    /// <summary>
    /// Represents provider of read and write streams
    /// for storage.
    /// </summary>
    public interface IStorageStreamProvider
    {
        /// <summary>
        /// Checks if storage exists.
        /// </summary>
        bool StorageExists { get; }
        /// <summary>
        /// Gets stream for reading from storage.
        /// </summary>
        Stream GetReadStream();
        /// <summary>
        /// Gets stream for writing to storage.
        /// </summary>
        /// <returns></returns>
        Stream GetWriteStream();
    }
}
