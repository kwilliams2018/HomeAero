using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAero.Interfaces
{
    interface IDeviceConfiguration
    {
        string SerialNumber { get; set; }
        string SoftwareVersion { get; set; }
    }
}
