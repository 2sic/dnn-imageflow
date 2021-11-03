using System;

namespace ToSic.Imageflow.Dnn.Job
{
    internal struct ImageData
    {
        public ArraySegment<byte> ResultBytes;
        public string ContentType;
    }
}
