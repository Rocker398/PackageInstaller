using PackageInstaller.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller.Services
{
    public class PackageInstallService
    {
        /// <summary>
        /// Install the given packages in the correct order to avoid missing dependencies
        /// </summary>
        /// <param name="packages">The list of packages with their dependencies</param>
        /// <returns>A response object that contains the status of the install and a comma separated string of package names in the order of install, such that a package's dependency will always precede that package</returns>
        public static PackageInstallResponse InstallPackages(List<string> packages)
        {
            PackageInstallResponse response = new PackageInstallResponse();

            string installedPackages = string.Empty;
            
            // Install the packages

            response.InstalledPackages = installedPackages;
            return response;
        }
    }
}
