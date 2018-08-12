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
            // Exit the program if no input was given.
            if (args.Length == 0)
            {
                Console.WriteLine("There is nothing to install, exiting program");

                return;
            }

            // Create the list of packages and determine the order of install based on dependencies
            List<string> packages = new List<string>(args);

            string installedPackages = PackageInstallService.InstallPackages(packages);

            if (string.IsNullOrWhiteSpace(installedPackages))
            {
                Console.WriteLine("No Packages were installed, invalid dependency specification");
            }
        }
    }
}
