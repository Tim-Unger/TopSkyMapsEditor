using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

            string[] vorArray = SplitArray(lines, "[VOR]");
            string[] ndbArray = SplitArray(lines, "[NDB]");
            string[] fixesArray = SplitArray(lines, "[FIXES]");
            string[] airportArray = SplitArray(lines, "[AIRPORT]");
            string[] runwayArray = SplitArray(lines, "[RUNWAY]");

            //Find all VORs
            sctFile.vors = ReadVors(vorArray);

            //Find all NDBs
            sctFile.ndbs = ReadNdbs(ndbArray);

            //Find all Fixes
            sctFile.fixes = ReadFixes(fixesArray);

            //Find all Runways
            List<Runway> runwayList = ReadRunways(runwayArray);

            //Find all Airports
            sctFile.airports = ReadAirports(airportArray, runwayList);

            StopWatch.Stop();
            var Time = StopWatch.ElapsedMilliseconds;
            return sctFile;
        }

        private static string[] SplitArray(string[] lines, string search)
        {
            int index = Array.FindIndex(lines, content => content.StartsWith(search)) + 1;

            var array = lines[index..];

            return array;
        }

        private static List<Vor> ReadVors(string[] vorArray)
        {
            List<Vor> vorList = new List<Vor>();
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

                    vorList.Add(vor);
                    continue;
                }

                break;
            }

            return vorList;
        }

        private static List<Ndb> ReadNdbs(string[] ndbArray)
        {
            List<Ndb> ndbList = new List<Ndb>();
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

                    ndbList.Add(ndb);
                    continue;
                }

                break;
            }

            return ndbList;
        }

        private static List<Runway> ReadRunways(string[] runwayArray)
        {
            var runwayList = new List<Runway>();    
            foreach (var runwayLine in runwayArray)
            {
                if (!runwayLine.StartsWith("["))
                {
                    var runway = new Runway();

                    Regex RunwayRegex = new Regex(
                        "(([0-9]{1,2})(L|R|C)?)\\s{1,}(([0-9]{1,2})(L|R|C)?)\\s{1,}([0-9]{3})\\s{1,}([0-9]{3})\\s{1,}((N|E)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3})\\s{1,}((E|W)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3})\\s{1,}((N|E)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3})\\s{1,}((E|W)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3})\\s{1,}([A-Z]{4})"
                    );

                    MatchCollection runwayMatches = RunwayRegex.Matches(runwayLine);
                    if (runwayMatches.Count == 1)
                    {
                        GroupCollection groups = runwayMatches[0].Groups;

                        runway.FirstRunwayIndentifier = groups[1].Value;
                        runway.SecondRunwayIdentifier = groups[4].Value;
                        runway.FirstRunwayDirection = int.Parse(groups[7].Value);
                        runway.SecondRunwayDirection = int.Parse(groups[8].Value);
                        runway.FirstRunwayStartCoordinate = groups[9].Value;
                        runway.FirstRunwayEndCoordinate = groups[11].Value;
                        runway.SecondRunwayStartCoordinate = groups[13].Value;
                        runway.SecondRunwayEndCoordinate = groups[15].Value;
                        runway.Airport = groups[17].Value;
                    }

                    runwayList.Add(runway);
                    continue;
                }

                break;
            }

            return runwayList;
        }

        private static List<Airport> ReadAirports(string[] airportArray, List<Runway> runwayList)
        {
            List<Airport> airportList = new List<Airport>();   
            foreach (var airportLine in airportArray)
            {
                if (!airportLine.StartsWith("["))
                {
                    var airport = new Airport();

                    var RunwayList = new List<Runway>();

                    Regex AirportRegex = new Regex(
                        @"([A-Z]{4})\s{1,}(\S{1,})\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})"
                    );

                    MatchCollection airportMatches = AirportRegex.Matches(airportLine);
                    if (airportMatches.Count == 1)
                    {
                        GroupCollection groups = airportMatches[0].Groups;

                        airport.Name = groups[1].Value;
                        airport.Frequency = groups[2].Value;
                        airport.Coordinates = groups[3].Value;
                    }

                    foreach (var Runway in runwayList)
                    {
                        if (Runway.Airport == airport.Name)
                        {
                            RunwayList.Add(Runway);
                        }
                    }
                    airport.runways = RunwayList;

                    airportList.Add(airport);
                    continue;
                }
                break;
            }

            return airportList;
        }

        private static List<Fix> ReadFixes(string[] fixesArray)
        {
            List<Fix> fixList = new List<Fix>();
            foreach (var fixLine in fixesArray)
            {
                if (!fixLine.StartsWith("["))
                {
                    var fix = new Fix();

                    Regex FixRegex = new Regex(
                        "(\\S{1,})\\s{1,}((N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}\\s{1,})(E|W)([0-9]{3}).[0-9]{2}.[0-9]{2}.[0-9]{3})"
                    );

                    MatchCollection fixMatches = FixRegex.Matches(fixLine);
                    if (fixMatches.Count == 1)
                    {
                        GroupCollection groups = fixMatches[0].Groups;

                        fix.Name = groups[1].Value;
                        fix.Coordinates = groups[2].Value;
                    }

                    fixList.Add(fix);
                    continue;
                }

                break;
            }

            return fixList;
        }
    }
}
