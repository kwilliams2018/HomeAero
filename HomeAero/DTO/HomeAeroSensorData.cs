using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAero.DTO
{
    public class HomeAeroSensorData
    {
        public double RootTemperature { get; set; }
        public double RootHumidity { get; set; }
        public double PlantTemperature { get; set; }
        public double PlantHumidity { get; set; }
    }
}
