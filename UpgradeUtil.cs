using System.IO;
using System.Web;

namespace ToSic.Imageflow.Dnn
{
    internal static class UpgradeUtil
    {
        public static bool Upgraded { get; private set; }

        // Replace native assemblies
        private static readonly string[] Files = {
            @"runtimes\win-x64\native\imageflow.dll",
            @"runtimes\win-x86\native\imageflow.dll"
        };

        private static readonly object UpgradeLock = new object();
        private static string Bin { get; } = HttpContext.Current.Server.MapPath(@"~\bin\");
        private static string Temp { get; } = Path.Combine(Bin, "_temp_"); // Dnn install should place native assemblies in this folder

        /// <summary>
        /// Ensure that native assemblies are in place, before we start to use them.
        /// </summary>
        public static void UpgradeNativeAssemblies()
        {
            // ensure that upgrade is executed only ounce
            if (Upgraded) return;

            lock (UpgradeLock)
            {
                // after waiting...
                if (!Upgraded) Upgraded = !Directory.Exists(Temp);

                // if upgrade is already done before and "temp" is missing, than no work
                if (Upgraded) return;

                // this part is tricky...
                var withoutExceptions = ReplaceNativeAssemblies();
                if (!withoutExceptions) return;

                // remove "temp" folder as persistent flag that upgrade is done
                Upgraded = DeleteTempFolder();
            }
        }

        /// <summary>
        /// Replacing of native assemblies is only possible when this assemblies are not already locked,
        /// because are in use and loaded.
        /// This part is tricky and it is possible that it will be executed more times, until all work is done.
        /// </summary>
        /// <returns>is executed without exception</returns>
        private static bool ReplaceNativeAssemblies()
        {
            var withoutExceptions = true;

            foreach (var file in Files)
            {
                var sourceFile = Path.Combine(Temp, file);
                if (!File.Exists(sourceFile)) continue;

                try
                {
                    var destinationFile = Path.Combine(Bin, file);

                    var directoryName = Path.GetDirectoryName(destinationFile);
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);

                    if (File.Exists(destinationFile)) File.Delete(destinationFile);

                    File.Move(sourceFile, destinationFile);
                }
                catch
                {
                    Upgraded = false;
                    withoutExceptions = false;
                }
            }

            return withoutExceptions;
        }

        /// <summary>
        /// "temp" folder is removed as persistent flag that upgrade is done.
        /// </summary>
        /// <returns>is deleted</returns>
        private static bool DeleteTempFolder()
        {
            // Delete "temp" folder when upgrade is without exceptions.
            try
            {
                Directory.Delete(Temp, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
