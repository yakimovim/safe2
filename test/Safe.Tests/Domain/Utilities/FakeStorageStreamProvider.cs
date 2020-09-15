using Safe.Core.Services;
using System.IO;

namespace Safe.Tests.Domain.Utilities
{
    public class FakeStorageStreamProvider : IStorageStreamProvider
    {
        private MemoryStream _stream;

        public bool StorageExists { get; set; }

        public Stream GetReadStream()
        {
            if(_stream != null)
            {
                var buffer = _stream.ToArray();

                return new MemoryStream(buffer);
            }

            return new MemoryStream();
        }

        public Stream GetWriteStream()
        {
            _stream = new MemoryStream();

            return _stream;
        }
    }
}
