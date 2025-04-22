using Imageflow.Fluent;
using Imazen.Common.Storage;
using System;
using System.Threading.Tasks;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal class BlobFetchResult : IDisposable
    {
        private IAsyncMemorySource _memorySource;
        private IBlobData _blob;

        public IAsyncMemorySource GetBytesSource() => _memorySource;

        public static async Task<BlobFetchResult> FromCache(BlobFetchCache blobFetchCache)
        {
            using (var blob = await blobFetchCache.GetBlob())
            {
                if (blob == null) return null;

                var memorySource = BufferedStreamSource.BorrowStreamRemainder(blob.OpenRead());
                return new BlobFetchResult()
                {
                    _memorySource = memorySource,
                    _blob = blob
                };
            }
        }

        public void Dispose()
        {
            _memorySource?.Dispose();
            _blob?.Dispose();
        }
    }
}
