using System;
using System.Threading.Tasks;
using Imazen.Common.Storage;

namespace ToSic.Imageflow.Dnn
{
    internal struct BlobProviderResult
    {
        internal bool IsFile;
        internal Func<Task<IBlobData>> GetBlob;
    }
}
