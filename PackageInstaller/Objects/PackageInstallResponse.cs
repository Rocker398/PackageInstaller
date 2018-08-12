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
        /// If install successful, contains the comma separated list of packages and the order they were installed
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
