using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller.Objects
{
    public class PackageInfo
    {
        public string Name { get; set; }

        public string Dependency { get; set; }

        public PackageInfo()
        {
            this.Name = string.Empty;
            this.Dependency = string.Empty;
        }
    }
}
