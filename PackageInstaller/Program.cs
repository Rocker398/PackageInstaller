using PackageInstaller.Enums;
using PackageInstaller.Objects;
using PackageInstaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Allow package input as arguments or wait for it in the program
            string[] packages = args;
            if (args.Length == 0)
            {
                try
                {
                    /*
                     * Accepts input in any of the following formats:
                     *      [ "PackageName: Dependency", "PackageName: " ]
                     *      "PackageName: Dependency", "PackageName: "
                     *      PackageName: Dependency, PackageName: 
                     */
                    string input = Console.ReadLine();

                    packages = PackageInstallService.GetCleanedPackageListFromInput(input);
                }
                catch
                {
                    Console.WriteLine("Invalid input given, exiting program");

                    return;
                }
            }
                        
            // Exit the program if no valid input was given.
            if (packages.Length == 0)
            {
                Console.WriteLine("There is nothing to install, exiting program");

                return;
            }

            // Install the given packages based on their dependencies
            PackageInstallResponse response = PackageInstallService.InstallPackages(packages);

            // Build a message based on the result of the install
            string message = string.Empty;
            switch (response.Status)
            {
                case PackageInstallStatuses.SUCCESS:
                    {
                        message = response.InstalledPackages;
                        break;
                    }

                case PackageInstallStatuses.CONTAINS_CYCLE:
                    {
                        message = "No Packages were installed, invalid dependency specification that contains cycles.";
                        break;
                    }

                case PackageInstallStatuses.ERROR:
                case PackageInstallStatuses.DEFAULT_NOT_SET:
                default:
                    {
                        message = "No Packages were installed, unknown error occurred.";
                        break;
                    }
            }
            
            // Print the result to the console
            Console.WriteLine(message);
        }
    }
}
