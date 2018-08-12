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

            // Parse packages to a list of PackageInfo objects
            List<PackageInfo> packageInfoList = GetPackageInfoList(packages);
            
            List<PackageInfo> installedPackages = new List<PackageInfo>();

            // Get and install the packages with no dependencies first
            List<PackageInfo> packagesWithNoDependencies = packageInfoList.Where(x => x.Dependency == string.Empty).ToList();

            // Filter the packages to exclude the ones with no dependencies
            packageInfoList = packageInfoList.Where(x => x.Dependency != string.Empty).ToList();
            
            /*
             * While packagesWithNoDependencies is non-empty
             * {
             *      remove package from packagesWithNoDependencies
             *      add package to installedPackages
             *      
             *      for each pkg with a dependency on the current package
             *      {
             *          remove pkg from packageInfoList
             *          if pkg has no other dependencies
             *          {
             *              add pkg to packagesWithNoDependencies
             *          }
             *      }
             * }
             * 
             * If packageInfoList has packages
             * {
             *      the graph has cycles
             * }
             * else
             * {
             *      return installedPackages
             * }
             */
             
            // Topographical sort
            while (packagesWithNoDependencies.Count > 0)
            {
                PackageInfo currPackage = packagesWithNoDependencies.First();
                packagesWithNoDependencies.Remove(currPackage);
                installedPackages.Add(currPackage);

                List<PackageInfo> packagesWithCurrentDependency = packageInfoList.Where(x => x.Dependency == currPackage.Name).ToList();
                foreach (PackageInfo package in packagesWithCurrentDependency)
                {
                    packageInfoList.Remove(package);

                    List<PackageInfo> packagesWithDependency = packageInfoList.Where(x => x.Name == package.Dependency).ToList();
                    //List<PackageInfo> packagesWithDependency = packageInfoList.Where(x => x.Dependency == package.Name).ToList();

                    if (packagesWithDependency.Count == 0)
                    {
                        packagesWithNoDependencies.Add(package);
                    }
                }
            }

            if (packageInfoList.Count > 0)
            {
                response.Status = PackageInstallStatuses.CONTAINS_CYCLE;
            }
            else
            {
                string installedPackagesFormatted = GetInstalledPackagesFormatted(installedPackages);

                response.Status = PackageInstallStatuses.SUCCESS;
                response.InstalledPackages = installedPackagesFormatted;
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

        private static string GetInstalledPackagesFormatted(List<PackageInfo> packages)
        {
            string installedPackages = string.Empty;

            List<string> packageNames = packages.Select(x => x.Name).ToList();
            installedPackages = string.Join(", ", packageNames);

            return installedPackages;
        }
    }
}
