using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopskyMapsEditor
{
    public class Ese
    {
        public static List<Position> Positions { get; set; }

    }

    public class Position
    {
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public string SectorIndicator { get; set; }
    }
}
