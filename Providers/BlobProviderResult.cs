using System;
using System.Threading.Tasks;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn.Providers
{
    internal struct BlobProviderResult
    {
        public bool IsFile;
        public Func<Task<IBlobData>> GetBlob;
    }
}
