using System;
using System.Threading;
using System.Threading.Tasks;
using Imageflow.Fluent;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn.Providers
{
    public class BlobFetchResult : IDisposable
    {
        private IBlobData blob;
        private StreamSource streamSource;
        private ArraySegment<byte> bytes;


        internal BytesSource GetBytesSource()
        {
            return new BytesSource(bytes);
        }
        public void Dispose()
        {
            streamSource?.Dispose();
            blob?.Dispose();
        }

        public static async Task<BlobFetchResult> FromCache(BlobFetchCache blobFetchCache)
        {
            using (var blob = await blobFetchCache.GetBlob())
            {
                if (blob == null) return null;

                var source = new StreamSource(blob.OpenRead(), true);
                var result = new BlobFetchResult()
                {
                    streamSource = source,
                    blob = blob,
                    bytes = await source.GetBytesAsync(CancellationToken.None)
                };
                return result;
            }
        }
    }
}
