using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageInstaller.Enums
{
    public enum PackageInstallStatuses
    {
        DEFAULT_NOT_SET = 0,
        SUCCESS = 1,
        CONTAINS_CYCLE = 2
    }
}
