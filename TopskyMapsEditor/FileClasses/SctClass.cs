using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopskyMapsEditor
{
    public class SctFile
    {
        public List<Vor>? vors { get; set; }
        public List<Ndb>? ndbs { get; set; }
        public List<Fix> fixes { get; set; }
        public List<Airport> airports { get; set; }
    }

    public class Vor
    {
        public string Name { get; set; }
        public string Frequency { get; set; }
        public string Coordinates { get; set; }
    }

    public class Ndb
    {
        public string Name { get; set; }
        public string Frequency { get; set; }
        public string Coordinates { get; set; }
    }
    public class Fix
    {
        public string Name { get; set; }
        public string Coordinates { get; set; }
    }
    public class Airport
    {
        public string Name { get; set; }

        //?? idk
        public string Frequency { get; set; }
        public string Coordinates { get; set; }

        public List<Runway> runways { get; set; }
    }

    public class Runway
    {
        public string FirstRunwayIndentifier { get; set; }
        public string SecondRunwayIdentifier { get; set; }
        public int FirstRunwayDirection { get; set; }
        public int SecondRunwayDirection { get;set; }

        public string FirstRunwayStartCoordinate { get; set; }
        public string FirstRunwayEndCoordinate { get; set; }
        public string SecondRunwayStartCoordinate { get; set; }
        public string SecondRunwayEndCoordinate { get; set; }
        public string Airport { get; set; }

    }
}
