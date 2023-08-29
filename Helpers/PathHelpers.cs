using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ToSic.Imageflow.Dnn.Helpers
{
    internal static class PathHelpers
    {
        private static readonly string[] AcceptedImageExtensions = {
            ".png",
            ".jpg",
            ".jpeg",
            ".jfif",
            ".jif",
            ".jfi",
            ".jpe",
            ".gif",
            ".webp"
        };

        public static bool IsImagePath(string path)
            => AcceptedImageExtensions.Any(imageFileExtension => path.EndsWith(imageFileExtension, StringComparison.OrdinalIgnoreCase));

        public static readonly string[] SupportedQuerystringKeys = {
            "mode", "anchor", "flip", "sflip", "scale", "cache", "process",
            "quality", "zoom", "dpr", "crop", "cropxunits", "cropyunits",
            "w", "h", "width", "height", "maxwidth", "maxheight", "format", "thumbnail",
            "autorotate", "srotate", "rotate", "ignoreicc",
            "stretch", "webp.lossless", "webp.quality",
            "frame", "page", "subsampling", "colors", "f.sharpen", "f.sharpen_when", "down.colorspace",
            "404", "bgcolor", "paddingcolor", "bordercolor", "preset", "floatspace", "jpeg_idct_downscale_linear", "watermark",
            "s.invert", "s.sepia", "s.grayscale", "s.alpha", "s.brightness", "s.contrast", "s.saturation", "trim.threshold",
            "trim.percentpadding", "a.blur", "a.sharpen", "a.removenoise", "a.balancewhite", "dither", "jpeg.progressive",
            "encoder", "decoder", "builder", "s.roundcorners.", "paddingwidth", "paddingheight", "margin", "borderwidth", "decoder.min_precise_scaling_ratio"
        };

        /// <summary>
        /// Return the Base64 encoded hash string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64Hash(string data)
        {
            // Create an instance of the SHA256 hashing algorithm.
            using (var sha2 = SHA256.Create())
            {
                // Get the bytes of the input string.
                var stringBytes = Encoding.UTF8.GetBytes(data);

                // Compute the hash of bytes using the SHA256 algorithm
                var hashBytes = sha2.ComputeHash(stringBytes);

                // Convert the hash bytes to a Base64 encoded string.
                var base64String = Convert.ToBase64String(hashBytes);

                // Remove the padding characters and any URL encoding characters
                base64String = base64String.Replace("=", string.Empty)
                                           .Replace('+', '-')
                                           .Replace('/', '_');

                
                return base64String;
            }
        }

        ///<summary>
        ///Create a dictionary from a name value collection.
        ///</summary>
        ///<returns>A dictionary containing all the key-value pairs from a NameValueCollection</returns>
        ///<param name="requestQuery">The NameValueCollection containing the input key-value pairs.</param>
        public static Dictionary<string, string> ToQueryDictionary(NameValueCollection requestQuery)
        {
            var dict = new Dictionary<string, string>(requestQuery.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var key in requestQuery.AllKeys) dict.Add(key, requestQuery[key]);
            return dict;
        }

        public static string SerializeCommandString(Dictionary<string, string> finalQuery)
        {
            var sb = new StringBuilder();
            foreach (var key in finalQuery.Keys.Where(k => !string.IsNullOrEmpty(k)))
                sb.Append($"&{key}={HttpUtility.UrlEncode(finalQuery[key])}");
            return sb.ToString()?.TrimStart('&');
        }
    }
}
