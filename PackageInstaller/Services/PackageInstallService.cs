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
        public static PackageInstallResponse InstallPackages(string[] packages)
        {
            PackageInstallResponse response = new PackageInstallResponse();

            // Parse packages to a list of PackageInfo objects
            List<PackageInfo> packageInfoList = GetPackageInfoList(packages);

            // TODO: determine the order of the packages to install based on dependencies

            string installedPackages = string.Empty;
            
            response.InstalledPackages = installedPackages;
            return response;
        }

        /// <summary>
        /// Parse the input list of packages and build the list of PackageInfo objects to be able to access the details easier
        /// </summary>
        /// <param name="packages">The list of packages and their dependencies separated by ":"</param>
        /// <returns></returns>
        private static List<PackageInfo> GetPackageInfoList(string[] packages)
        {
            List<PackageInfo> packageInfoList = new List<PackageInfo>();

            for (int i = 0; i < packages.Length; i++)
            {
                string currentPackage = packages[i];
                string[] packageDetails = currentPackage.Split(':');

                // If nothing to add, continue
                if (packageDetails.Length == 0)
                {
                    continue;
                }

                string packageName = packageDetails[0];
                string packageDependency = packageDetails[1].Trim();

                PackageInfo packageInfo = new PackageInfo()
                {
                    Name = packageName,
                    Dependency = packageDependency
                };

                packageInfoList.Add(packageInfo);
            }

            return packageInfoList;
        }
    }
}
