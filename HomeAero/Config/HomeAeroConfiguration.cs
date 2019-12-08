using HomeAero.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAero.Config
{
    public class HomeAeroConfiguration : IDeviceConfiguration
    {
        private string _SerialNumber;
        private string _SoftwareVersion;

        public string SerialNumber { get => _SerialNumber; set { _SerialNumber = value; } }
        public string SoftwareVersion { get => _SoftwareVersion; set { _SoftwareVersion = value; } }
    }
}
