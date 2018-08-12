using PackageInstaller.Enums;
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

            try
            {
                // Parse packages to a list of PackageInfo objects
                List<PackageInfo> packageInfoList = GetPackageInfoList(packages);

                List<PackageInfo> installedPackages = new List<PackageInfo>();

                // Get and install the packages with no dependencies first
                List<PackageInfo> installQueue = packageInfoList.Where(x => x.Dependency == string.Empty).ToList();

                // Filter the packages to exclude the ones with no dependencies
                packageInfoList = packageInfoList.Where(x => x.Dependency != string.Empty).ToList();

                // Perform a topological sort, while the install queue is not empty
                while (installQueue.Count > 0)
                {
                    // Remove the first package from the install queue, and install the package
                    PackageInfo currPackage = installQueue.First();
                    installQueue.Remove(currPackage);
                    installedPackages.Add(currPackage);

                    // Find all packages that have a dependency on the current package and add them to the install queue
                    List<PackageInfo> packagesWithCurrentDependency = packageInfoList.Where(x => x.Dependency == currPackage.Name).ToList();
                    foreach (PackageInfo package in packagesWithCurrentDependency)
                    {
                        // We are handling the package, so remove it from the original "graph" i.e. packageInfoList
                        packageInfoList.Remove(package);

                        // Because packages only have one dependency, we know the current package dependencies are already installed, so just add this to the install queue
                        installQueue.Add(package);
                    }
                }

                // If there are still items in the original list, the package dependencies contain a cycle
                if (packageInfoList.Count > 0)
                {
                    response.Status = PackageInstallStatuses.CONTAINS_CYCLE;
                }
                else
                {
                    // The package dependencies did not contain a cycle, get the formatted string of the install order
                    string installedPackagesFormatted = GetInstalledPackagesFormatted(installedPackages);

                    response.Status = PackageInstallStatuses.SUCCESS;
                    response.InstalledPackages = installedPackagesFormatted;
                }
            }
            catch
            {
                // If given incorrectly formatted input, we will break here and just return an error status
                response.Status = PackageInstallStatuses.ERROR;
            }

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

        /// <summary>
        /// Given a list of packages in the order to be installed, returns a comma separated string of packages
        /// </summary>
        /// <param name="packages">A list of Packages in the order to be installed</param>
        /// <returns>A comma separated string of package names in the order of install, such that a package's dependency will always precede that package</returns>
        private static string GetInstalledPackagesFormatted(List<PackageInfo> packages)
        {
            string installedPackages = string.Empty;

            List<string> packageNames = packages.Select(x => x.Name).ToList();

            if (packageNames.Count > 0)
            {
                installedPackages = string.Join(", ", packageNames);
            }

            return installedPackages;
        }
    }
}
