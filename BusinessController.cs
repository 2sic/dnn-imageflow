using DotNetNuke.Entities.Modules;

namespace ToSic.Imageflow.Dnn
{
    /// <summary>
    /// BusinessController to support UpgradeModule
    /// </summary>
    public class BusinessController : IUpgradeable
    {
        /// <summary>
        /// Executed on module upgrade.
        /// This Library package have dnn manifest with simplified "module" component,
        /// to specify this BusinessController that supports upgrade.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public string UpgradeModule(string version)
        {
            UpgradeUtil.UpgradeNativeAssemblies();
            return $"Upgraded: {UpgradeUtil.Upgraded}";
        }
    }
}
