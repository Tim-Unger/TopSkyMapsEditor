using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TopskyMapsEditor;

namespace Parser
{
    public class SctClass
    {
        public static SctFile ReadSctFile(string Raw)
        {
            //Just to test Performance
            var StopWatch = System.Diagnostics.Stopwatch.StartNew();

            SctFile sctFile = new SctFile();

            List<Vor> VorList = new List<Vor>();
            List<Ndb> NdbList = new List<Ndb>();
            List<Fix> FixList = new List<Fix>();
            List<Airport> AirportList = new List<Airport>();

            string[] lines = Raw.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            int vorStartIndex = Array.FindIndex(lines, content => content.StartsWith("[VOR]")) + 1;
            int ndbStartIndex = Array.FindIndex(lines, content => content.StartsWith("[NDB]")) + 1;
            int fixesStartIndex =
                Array.FindIndex(lines, content => content.StartsWith("[FIXES]")) + 1;
            int airportStartIndex =
                Array.FindIndex(lines, content => content.StartsWith("[AIRPORT]")) + 1;

            int runwayStartIndex = Array.FindIndex(lines, content => content.StartsWith("[RUNWAY]")) + 1;

            var vorArray = lines[vorStartIndex..];
            var ndbArray = lines[ndbStartIndex..];
            var fixesArray = lines[fixesStartIndex..];
            var airportArray = lines[airportStartIndex..];
            var runwayArray = lines[runwayStartIndex..];

            //Find all VORs
            foreach (var vorLine in vorArray)
            {
                if (!vorLine.StartsWith("["))
                {
                    Vor vor = new Vor();

                    Regex vorRegex = new Regex(
                        @"(\S{1,})\s{1,}([0-9]{3}.[0-9]{3})\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})"
                    );

                    MatchCollection vorMatches = vorRegex.Matches(vorLine);
                    if (vorMatches.Count == 1)
                    {
                        GroupCollection groups = vorMatches[0].Groups;

                        vor.Name = groups[1].Value;
                        vor.Frequency = groups[2].Value;
                        vor.Coordinates = groups[3].Value;
                    }

                    VorList.Add(vor);
                }
                else
                {
                    break;
                }
            }

            //Find all NDBs
            foreach (var ndbLine in ndbArray)
            {
                if (!ndbLine.StartsWith("["))
                {
                    var ndb = new Ndb();

                    Regex NdbRegex = new Regex(
                        @"(\S{1,})\s{1,}([0-9]{3}.[0-9]{3})\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})"
                    );

                    MatchCollection ndbMatches = NdbRegex.Matches(ndbLine);
                    if (ndbMatches.Count == 1)
                    {
                        GroupCollection groups = ndbMatches[0].Groups;

                        ndb.Name = groups[1].Value;
                        ndb.Frequency = groups[2].Value;
                        ndb.Coordinates = groups[3].Value;
                    }

                    NdbList.Add(ndb);
                }
                else
                {
                    break;
                }
            }

            //Find all Fixes
            foreach (var fixLine in fixesArray)
            {
                if (!fixLine.StartsWith("["))
                {
                    var fix = new Fix();

                    Regex FixRegex = new Regex("(\\S{1,})\\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})");

                    MatchCollection fixMatches = FixRegex.Matches(fixLine);
                    if(fixMatches.Count == 1)
                    {
                        GroupCollection groups = fixMatches[0].Groups;

                        fix.Name = groups[1].Value;
                        fix.Coordinates = groups[2].Value;
                    }

                    FixList.Add(fix);
                }
                else
                {
                    break;
                }
            }

            //Find all Airports
            foreach (var airportLine in airportArray)
            {
                if (!airportLine.StartsWith("["))
                {
                    var airport = new Airport();

                    Regex AirportRegex = new Regex(@"([A-Z]{4})\s{1,}(\S{1,})\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})");

                    MatchCollection airportMatches = AirportRegex.Matches(airportLine);
                    if(airportMatches.Count == 1)
                    {
                        GroupCollection groups = airportMatches[0].Groups;

                        airport.Name = groups[1].Value;
                        airport.Frequency = groups[2].Value;
                        airport.Coordinates = groups[3].Value;
                    }

                    AirportList.Add(airport);
                }
                else
                {
                    break;
                }
            }

            sctFile.vors = VorList;
            sctFile.ndbs = NdbList;
            sctFile.fixes = FixList;
            sctFile.airports = AirportList;

            StopWatch.Stop();
            var Time = StopWatch.ElapsedMilliseconds;
            return sctFile;
        }
    }
}
