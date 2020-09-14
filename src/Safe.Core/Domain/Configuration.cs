namespace Safe.Core.Domain
{
    /// <summary>
    /// Represents read-only configuration of the program.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Salt for generation of encryption key from password.
        /// </summary>
        byte[] Salt { get; } 
        /// <summary>
        /// Language of the program.
        /// </summary>
        string Language { get; }
        /// <summary>
        /// Path to the storage of data.
        /// </summary>
        string StoragePath { get; }
    }

    /// <summary>
    /// Represents configuration of the program.
    /// </summary>
    public class Configuration : IConfiguration
    {
        /// <inheritdoc />
        public string Language { get; set; }
        /// <inheritdoc />
        public string StoragePath { get; set; }
        /// <inheritdoc />
        public byte[] Salt { get; set; }
    }
}
