using System.Threading.Tasks;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal class BlobFetchCache
    {
        public BlobFetchCache(string virtualPath, BlobProvider provider)
        {
            _virtualPath = virtualPath;
            _provider = provider;
            _resultFetched = false;
            _blobFetched = false;
        }

        private readonly BlobProvider _provider;
        private readonly string _virtualPath;
        private bool _resultFetched;
        private bool _blobFetched;
        private BlobProviderResult? _result;
        private IBlobData _blob;

        public BlobProviderResult? GetBlobResult()
        {
            if (_resultFetched) return _result;
            _result = _provider.GetResult(_virtualPath);
            _resultFetched = true;
            return _result;
        }

        public async Task<IBlobData> GetBlob()
        {
            if (_blobFetched) return _blob;
            var blobResult = GetBlobResult();
            if (blobResult != null) _blob = await blobResult.Value.GetBlob();
            _blobFetched = true;
            return _blob;
        }
    }
}
