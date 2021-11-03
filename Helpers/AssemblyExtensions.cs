using System;
using System.IO;
using System.Reflection;

namespace ToSic.Imageflow.Dnn.Helpers
{
    internal static class AssemblyExtensions
    {
        private static T GetFirstAttribute<T>(this ICustomAttributeProvider a)
        {
            try
            {
                var attrs = a.GetCustomAttributes(typeof(T), false);
                if (attrs.Length > 0) return (T)attrs[0];
            }
            catch (FileNotFoundException)
            {
                //Missing dependencies
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { }
            return default;
        }

        public static Exception GetExceptionForReading<T>(this Assembly a)
        {
            try
            {
                var unused = a.GetCustomAttributes(typeof(T), false);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        public static string GetInformationalVersion(this Assembly a)
        {
            return GetFirstAttribute<AssemblyInformationalVersionAttribute>(a)?.InformationalVersion;
        }
        public static string GetFileVersion(this Assembly a)
        {
            return GetFirstAttribute<AssemblyFileVersionAttribute>(a)?.Version;
        }
    }
}
