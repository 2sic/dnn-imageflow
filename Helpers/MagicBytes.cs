using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Imageflow.Fluent;

namespace ToSic.Imageflow.Dnn.Helpers
{
    /// <summary>
    /// Identifying Common File Formats
    /// </summary>
    internal static class MagicBytes
    {
        public static string GetContentTypeFromBytes(byte[] data)
        {
            if (data.Length < 12)
            {
                return "application/octet-stream";
            }
            return ImageJob.GetContentTypeForBytes(data) ?? "application/octet-stream";
        }

        /// <summary>
        /// Proxies the given stream to the HTTP response, while also setting the content length
        /// and the content type based off the magic bytes of the image
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="response"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ProxyToStream(Stream sourceStream, HttpResponse response)
        {
            // We really only need 12 bytes but it would be a waste to only read that many. 
            const int bufferSize = 4096;
            var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                var bytesRead = await sourceStream.ReadAsync(buffer,0, buffer.Length).ConfigureAwait(false);
                if (bytesRead == 0)
                    throw new InvalidOperationException("Source blob has zero bytes.");

                response.ContentType = bytesRead >= 12 ? GetContentTypeFromBytes(buffer) : "application/octet-stream";
                response.BinaryWrite(buffer);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            await sourceStream.CopyToAsync(response.OutputStream).ConfigureAwait(false);
        }
    }
}
