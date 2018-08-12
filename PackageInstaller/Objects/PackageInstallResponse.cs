using PackageInstaller.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller.Objects
{
    /// <summary>
    /// An response object to contain information about the status and result of the install
    /// </summary>
    public class PackageInstallResponse
    {
        /// <summary>
        /// Determines the status of the install
        /// </summary>
        public PackageInstallStatuses Status { get; set; }

        /// <summary>
        /// If install successful, contains a comma separated string of package names in the order of install, such that a package's dependency will always precede that package
        /// </summary>
        public string InstalledPackages { get; set; }

        /// <summary>
        /// Constructor for the response object
        /// </summary>
        public PackageInstallResponse()
        {
            this.Status = PackageInstallStatuses.DEFAULT_NOT_SET;
            this.InstalledPackages = string.Empty;
        }
    }
}
