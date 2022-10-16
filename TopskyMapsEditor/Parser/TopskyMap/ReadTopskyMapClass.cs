using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using TopskyMapsEditor;

namespace Parser
{
    internal class TopskyMapClass
    {
        public static List<Folder> GetTopskyFolders(string Raw)
        {
            List<Folder> mapFolders = new List<Folder>();
            List<string> folderNames = new List<string>();

            //TODO
            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:)", RegexOptions.Multiline);

            MatchCollection mapMatches = mapRegex.Matches(Raw);

            //Get all the folders
            List<string> folders = new List<string>();
            foreach (Match match in mapMatches)
            {
                GroupCollection groups = match.Groups;
                string RegexValue = groups[0].Value;

                Regex folderRegex = new Regex("FOLDER:(.+)");

                MatchCollection folderMatches = folderRegex.Matches(RegexValue);

                GroupCollection folderGroups = folderMatches[0].Groups;

                if (!folders.Contains(folderGroups[1].Value))
                {
                    folders.Add(folderGroups[1].Value);
                }
            }

            //Add the correct maps to the corret folders
            foreach (var folder in folders)
            {
                Folder addFolder = new Folder();
                List<string> addFolderContent = new List<string>();
                addFolder.FolderName = folder;
                foreach (Match match in mapMatches)
                {
                    GroupCollection groups = match.Groups;
                    string RegexValue = groups[0].Value;

                    Regex folderRegex = new Regex("FOLDER:(.+)");
                    MatchCollection folderMatches = folderRegex.Matches(RegexValue);
                    GroupCollection folderGroups = folderMatches[0].Groups;

                    if (folderGroups[1].Value == folder)
                    {
                        Regex nameRegex = new Regex("MAP:(.+)");
                        MatchCollection nameMatches = nameRegex.Matches(RegexValue);
                        GroupCollection nameGroups = nameMatches[0].Groups;

                        addFolderContent.Add(nameGroups[1].Value);
                    }
                }
                addFolder.FolderContent = addFolderContent;
                mapFolders.Add(addFolder);
            }
            //foreach(var line in lines)
            //{
            //    Regex nameRegex = new Regex("MAP:(.{1,})");

            //    MatchCollection nameMatches = nameRegex.Matches(line);

            //    if(nameMatches.Count == 1)
            //    {
            //        GroupCollection groups = nameMatches[0].Groups;

            //        mapNames.Add(groups[1].Value);
            //    }
            //}

            return mapFolders;
        }

        public static List<string> GetTopskyMapNames(string raw)
        {
            List<string> topskyMaps = new List<string>();

            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:)", RegexOptions.Multiline);

            MatchCollection mapMatches = mapRegex.Matches(raw);

            //Get all the folders
            foreach (Match match in mapMatches)
            {
                string name = null;
                GroupCollection groups = match.Groups;

                string mapNameRegex = groups[0].Value;

                Regex nameRegex = new Regex("MAP:(.{1,})");

                MatchCollection nameMatches = nameRegex.Matches(mapNameRegex);

                if (nameMatches.Count == 1)
                {
                    GroupCollection nameGroups = nameMatches[0].Groups;

                    name = nameGroups[1].Value;
                }

                if (name != null)
                {
                    topskyMaps.Add(name);
                }
            }

            return topskyMaps;
        }

        public static List<TopskyMap> GetTopskyMaps(string raw)
        {
            var StopWatch = Stopwatch.StartNew();

            List<TopskyMap> topskyMaps = new List<TopskyMap>();

            //Gets all the TopskyMaps in the File
            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:|\Z)", RegexOptions.Multiline);
            MatchCollection mapMatches = mapRegex.Matches(raw);

            //Gets a single Topsky-Map and parses it
            foreach (Match match in mapMatches)
            {
                TopskyMap topskyMap = new TopskyMap();
                GroupCollection groups = match.Groups;

                topskyMap = ReadTopskyMap(match.ToString());

                topskyMaps.Add(topskyMap);
            }

            StopWatch.Stop();
            var Time = StopWatch.ElapsedMilliseconds;
            return topskyMaps;
        }

        internal static TopskyMap ReadTopskyMap(string content)
        {
            var StopWatch = Stopwatch.StartNew();

            TopskyMap topskyMap = new TopskyMap();

            //Get the Name
            Regex nameRegex = new Regex("^MAP:(.{1,})", RegexOptions.Multiline);
            MatchCollection nameMatches = nameRegex.Matches(content);
            if (nameMatches.Count == 1)
            {
                GroupCollection groups = nameMatches[0].Groups;
                topskyMap.Name = groups[1].Value.TrimEnd('\n', '\r');
            }

            //Get the Folder
            Regex folderRegex = new Regex("^FOLDER:(.{1,})", RegexOptions.Multiline);
            MatchCollection folderMatches = folderRegex.Matches(content);
            if (folderMatches.Count == 1)
            {
                GroupCollection groups = folderMatches[0].Groups;
                topskyMap.Folder = groups[1].Value.TrimEnd('\n', '\r');
            }

            //Get the Layer
            Regex layerRegex = new Regex("^LAYER:([0-9]{1,})", RegexOptions.Multiline);
            MatchCollection layerMatches = layerRegex.Matches(content);
            if (layerMatches.Count == 1)
            {
                GroupCollection groups = layerMatches[0].Groups;
                topskyMap.Layer = int.Parse(groups[1].Value.TrimEnd('\n', '\r'));
            }
            else if (layerMatches.Count == 0)
            {
                topskyMap.Layer = null;
            }

            //Get the Zoom
            Regex zoomRegex = new Regex("^ZOOM:([0-9]{1,})", RegexOptions.Multiline);
            MatchCollection zoomMatches = zoomRegex.Matches(content);
            if (zoomMatches.Count == 1)
            {
                GroupCollection groups = zoomMatches[0].Groups;
                topskyMap.Zoom = int.Parse(groups[1].Value.TrimEnd('\n', '\r'));
            }

            //Get the Active Types
            List<Active> activeList = new List<Active>();

            Regex activeTypeRegex = new Regex(
                "^ACTIVE:(1|[0-9]{4,6}|NOTAM|AREA|RWY|ID).*",
                RegexOptions.Multiline
            );
            MatchCollection activeTypeMatches = activeTypeRegex.Matches(content);

            if (activeTypeMatches.Count > 0)
            {
                foreach (Match match in activeTypeMatches)
                {
                    Active active = new Active();
                    GroupCollection groups = match.Groups;

                    string activeType = groups[1].Value;

                    bool isInt = int.TryParse(activeType, out int value);
                    //either always active or active by date
                    if (isInt)
                    {
                        //always active
                        if (value == 1)
                        {
                            active.ActiveType = ActiveType.Always;
                            active.IsAlwaysActive = true;
                        }
                        //active by date
                        else
                        {
                            List<string> weekdays = new List<string>
                            {
                                "Monday",
                                "Tuesday",
                                "Wednesday",
                                "Thursday",
                                "Friday",
                                "Saturday",
                                "Sunday"
                            };

                            string fullMatch = match.ToString();

                            Regex dateRegex = new Regex(
                                //"ACTIVE:((?<startYear>[0-9]{2})?(?<startMonth>[0-9]{2})(?<startDay>[0-9]{2})):((?<endYear>[0-9]{2})?(?<endMonth>[0-9]{2})(?<endDay>[0-9]{2})):(?<weekdays>[0-9]{1,7}):((?<startHour>[0-9]{2})(?<startMinute>[0-9]{2})):((?<endHour>[0-9]{2})(?<endMinute>[0-9]{2}))",
                                "ACTIVE:(([0-9]{2})?([0-9]{2})([0-9]{2})):(([0-9]{2})?([0-9]{2})([0-9]{2})):([0-9]{1,7}):(([0-9]{2})([0-9]{2})):(([0-9]{2})([0-9]{2}))",
                                RegexOptions.Multiline
                            );

                            MatchCollection dateMatches = dateRegex.Matches(fullMatch);

                            if (dateMatches.Count == 1)
                            {
                                GroupCollection dateGroups = dateMatches[0].Groups;

                                active.ActiveType = ActiveType.Time;
                                active.IsActiveByTime = true;

                                //Year is set
                                int startYear = 0;
                                if (dateGroups[2].Success == true)
                                {
                                    startYear = int.Parse(dateGroups[2].Value);
                                }
                                //Year not set

                                else
                                {
                                    startYear = DateTime.UtcNow.Year;
                                }
                                int startMonth = int.Parse(dateGroups[3].Value);
                                int startDay = int.Parse(dateGroups[4].Value);

                                int startHour = int.Parse(dateGroups[11].Value);
                                int startMinute = int.Parse(dateGroups[12].Value);

                                active.StartTime = new DateTime(
                                    startYear,
                                    startMonth,
                                    startDay,
                                    startHour,
                                    startMinute,
                                    00
                                );

                                int endYear = 0;
                                if (dateGroups[6].Success == true)
                                {
                                    endYear = int.Parse(dateGroups[6].Value);
                                }
                                else
                                {
                                    endYear = DateTime.UtcNow.Year;
                                }
                                int endMonth = int.Parse(dateGroups[7].Value);
                                int endDay = int.Parse(dateGroups[8].Value);

                                int endHour = int.Parse(dateGroups[14].Value);
                                int endMinute = int.Parse(dateGroups[15].Value);

                                active.EndTime = new DateTime(
                                    endYear,
                                    endMonth,
                                    endDay,
                                    endHour,
                                    endMinute,
                                    00
                                );

                                int weekdaysInt = int.Parse(dateGroups[9].Value);

                                if (weekdaysInt == 0)
                                {
                                    active.WeekDays = weekdays.ToArray();
                                }
                                else
                                {
                                    var weekdaysSingle = weekdaysInt.ToString().ToCharArray();

                                    List<string> weekdaysConverted = new List<string>();
                                    foreach (var weekday in weekdaysSingle)
                                    {
                                        int weekdayCoverted = (weekday - '0') - 1;
                                        weekdaysConverted.Add(weekdays[weekdayCoverted]);
                                    }

                                    active.WeekDays = weekdaysConverted.ToArray();
                                }
                            }
                        }
                    }
                    else
                    {
                        string fullMatch = match.ToString();
                        //Active by NOTAM
                        if (activeType == "NOTAM")
                        {
                            active.ActiveType = ActiveType.Notam;

                            active.IsActiveByNotam = true;

                            Regex notamRegex = new Regex("^ACTIVE:NOTAM:([A-Z]{4}):(.*)");
                            MatchCollection notamMatches = notamRegex.Matches(fullMatch);

                            if (notamMatches.Count == 1)
                            {
                                GroupCollection notamGroups = notamMatches[0].Groups;

                                active.NotamIcao = notamGroups[1].Value;
                                active.NotamText = notamGroups[2].Value;
                            }
                        }

                        //Active by Area
                        if (activeType == "AREA")
                        {
                            active.ActiveType = ActiveType.Area;
                            active.IsActiveByArea = true;

                            Regex areaRegex = new Regex("^ACTIVE:AREA:(.*)(:.*)?");

                            //TODO
                            MatchCollection areaMatches = areaRegex.Matches(fullMatch);

                            if (areaMatches.Count == 1)
                            {
                                GroupCollection areaGroups = areaMatches[0].Groups;
                            }
                        }

                        //Active by Runways
                        //TODO Comma separated list of runways?
                        //TODO fix class logic
                        if (activeType == "RWY")
                        {
                            active.ActiveType = ActiveType.Runway;
                            active.IsActiveByRunway = true;

                            Regex runwayRegex = new Regex(
                                "ACTIVE:RWY:ARR:(\\*|(([A-Z]{4})([0-9]{1,2})(L|R|C?)))(:(([A-Z]{4})([0-9]{1,2})(L|C|R)?))?:DEP:(\\*|(([A-Z]{4})([0-9]{1,2})(L|R|C?)))(:(([A-Z]{4})([0-9]{1,2}))(L|C|R)?)?"
                            );

                            MatchCollection runwayMatches = runwayRegex.Matches(fullMatch);

                            if (runwayMatches.Count == 1)
                            {
                                GroupCollection runwayGroups = runwayMatches[0].Groups;

                                ActiveRunway activeRunway = new ActiveRunway();
                                List<TopskyRunway> arrivalRunways = new List<TopskyRunway>();
                                List<TopskyRunway> notArrivalRunways = new List<TopskyRunway>();
                                List<TopskyRunway> departureRunways = new List<TopskyRunway>();
                                List<TopskyRunway> notDepartureRunways = new List<TopskyRunway>();

                                //List<ActiveRunway>? activeRunways = new List<ActiveRunway>();

                                //Arrival Runway
                                if (runwayGroups[1].Value != "*")
                                {
                                    TopskyRunway runway = new();

                                    runway.Icao = runwayGroups[3].Value;
                                    runway.Identifier =
                                        runwayGroups[4].Value + runwayGroups[5].Value;

                                    arrivalRunways.Add(runway);
                                }

                                //Not Arrival Runway
                                if (runwayGroups[6].Success == true && runwayGroups[6].Value != "*")
                                {
                                    TopskyRunway runway = new();

                                    runway.Icao = runwayGroups[8].Value;
                                    runway.Identifier =
                                        runwayGroups[9].Value + runwayGroups[10].Value;

                                    notArrivalRunways.Add(runway);
                                }

                                //Departure Runway
                                if (runwayGroups[11].Value != "*")
                                {
                                    TopskyRunway runway = new();

                                    runway.Icao = runwayGroups[13].Value;
                                    runway.Identifier =
                                        runwayGroups[14].Value + runwayGroups[15].Value;

                                    departureRunways.Add(runway);
                                }

                                if (
                                    runwayGroups[16].Success == true
                                    && runwayGroups[16].Value != "*"
                                )
                                {
                                    TopskyRunway runway = new();

                                    runway.Icao = runwayGroups[18].Value;
                                    runway.Identifier =
                                        runwayGroups[19].Value + runwayGroups[20].Value;

                                    notDepartureRunways.Add(runway);
                                }

                                activeRunway.ArrivalRunways = arrivalRunways;
                                activeRunway.NotArrivalRunways = notArrivalRunways;
                                activeRunway.DepartureRunways = departureRunways;
                                activeRunway.NotDepartureRunways = notDepartureRunways;

                                active.ActiveRunway = activeRunway;
                            }
                        }

                        //TODO
                        if (activeType == "ID") { }
                    }

                    activeList.Add(active);
                }
            }

            //Get the color
            //TODO Fill color
            Regex colorRegex = new Regex("COLOR:(.{1,})", RegexOptions.Multiline);
            MatchCollection colorMatches = colorRegex.Matches(content);
            if (colorMatches.Count == 1)
            {
                GroupCollection colorGroups = colorMatches[0].Groups;

                topskyMap.Color = colorGroups[1].Value;
            }

            //Get the Style
            Regex styleRegex = new Regex("STYLE:(\\S{1,}):([0-9]{1,})", RegexOptions.Multiline);
            MatchCollection styleMatches = styleRegex.Matches(content);
            if (styleMatches.Count == 1)
            {
                Style style = new Style();
                GroupCollection groups = styleMatches[0].Groups;

                switch (groups[1].Value.ToUpper())
                {
                    case "SOLID":
                        style.StyleType = StyleType.Solid;
                        break;
                    case "DASH":
                        style.StyleType = StyleType.Dash;
                        break;
                    case "DOT":
                        style.StyleType = StyleType.Dot;
                        break;
                    case "DASHDOT":
                        style.StyleType = StyleType.DashDot;
                        break;
                    case "DASHDOTDOT":
                        style.StyleType = StyleType.DashDotDot;
                        break;
                }

                int? width;
                if (groups[2].Success == true)
                {
                    width = int.Parse(groups[2].Value);
                }
                else
                {
                    width = null;
                }

                style.Width = width;

                topskyMap.Style = style;
            }

            //Get all Lines
            Regex lineRegex = new Regex(
                "LINE:(([A-Z]{2,5}):([A-Z]{2,5})|(((N|S)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|w)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((N|E)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|W)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}))|([A-Z]{4}/[0-9]{1,2}(L|C|R)?):([A-Z]{4}/[0-9]{1,2}(L|C|R)?))",
                RegexOptions.Multiline
            );

            MatchCollection lineMatches = lineRegex.Matches(content);

            if(lineMatches.Count > 0)
            {
                List<Line> lines = new();

                foreach(Match match in lineMatches)
                {
                    GroupCollection lineGroups = match.Groups;

                    Line line = new();

                    //Waypoints defined
                    if (lineGroups[2].Success == true)
                    {
                        line.StartPoint = lineGroups[2].Value;
                        line.EndPoint = lineGroups[3].Value;
                    }
                    //Runways defined
                    else if (lineGroups[13].Success == true) 
                    {
                        line.StartPoint = lineGroups[13].Value;
                        line.EndPoint = lineGroups[15].Value;
                    }
                    //Coordinates defined
                    else
                    {
                        line.StartLatitude = lineGroups[5].Value;
                        line.StartLongitude = lineGroups[7].Value;

                        line.EndLatitude = lineGroups[9].Value;
                        line.EndLongitude = lineGroups[11].Value;
                    }
                    lines.Add(line);
                }

                topskyMap.Lines = lines;
            }

            return topskyMap;
        }
    }
}
