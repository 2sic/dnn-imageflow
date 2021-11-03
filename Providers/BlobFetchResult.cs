using System;
using System.Threading;
using System.Threading.Tasks;
using Imageflow.Fluent;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal class BlobFetchResult : IDisposable
    {
        private IBlobData _blob;
        private StreamSource _streamSource;
        private ArraySegment<byte> _bytes;

        public BytesSource GetBytesSource()
        {
            return new BytesSource(_bytes);
        }

        public static async Task<BlobFetchResult> FromCache(BlobFetchCache blobFetchCache)
        {
            using (var blob = await blobFetchCache.GetBlob())
            {
                if (blob == null) return null;

                var source = new StreamSource(blob.OpenRead(), true);
                var result = new BlobFetchResult()
                {
                    _streamSource = source,
                    _blob = blob,
                    _bytes = await source.GetBytesAsync(CancellationToken.None)
                };
                return result;
            }
        }

        public void Dispose()
        {
            _streamSource?.Dispose();
            _blob?.Dispose();
        }
    }
}
