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

        //TODO Runways?
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
    }
}
