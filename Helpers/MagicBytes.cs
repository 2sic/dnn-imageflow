﻿using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace ToSic.Imageflow.Dnn.Helpers
{
    /// <summary>
    /// Identify common file formats and proxy streams to the HTTP response.
    /// </summary>
    internal static class MagicBytes
    {
        /// <summary>
        /// Returns the MIME type based on the provided byte array data.
        /// </summary>
        /// <param name="data">The byte array data to check for the MIME type.</param>
        /// <returns>A string representing the MIME type of the provided data.</returns>
        internal static string GetContentTypeFromBytes(byte[] data)
            => data.Length < 12
                ? "application/octet-stream"
                : new Imazen.Common.FileTypeDetection.FileTypeDetector().GuessMimeType(data) ?? "application/octet-stream";

        /// <summary>
        /// Proxies the given stream to the provided HTTP response, while also setting the content length
        /// and the content type based off the provided data if possible.
        /// </summary>
        /// <param name="sourceStream">The source stream to proxy.</param>
        /// <param name="response">The HTTP response to write to.</param>
        /// <exception cref="InvalidOperationException"></exception>
        internal static async Task ProxyToStream(Stream sourceStream, HttpResponse response)
        {
            // Response content length
            if (sourceStream.CanSeek)
                response.AddHeader("Content-Length", (sourceStream.Length - sourceStream.Position).ToString());

            // Prepare buffer to improve efficiency when reading from the source stream
            const int bufferSize = 4096;
            var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            // Response content type and image binary data starting part (from buffered data)
            try
            {
                // Read from the source stream into the buffer
                var bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                if (bytesRead == 0)
                    throw new InvalidOperationException("Source blob has zero bytes.");

                // Determine the content type based on the buffered data
                response.ContentType = bytesRead >= 12 ? GetContentTypeFromBytes(buffer) : "application/octet-stream";

                // Write the buffered image binary data to the response stream
                await response.OutputStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer, true);
            }

            // Response image binary data remaining part (from the source stream)
            await sourceStream.CopyToAsync(response.OutputStream).ConfigureAwait(false);
        }
    }
}
