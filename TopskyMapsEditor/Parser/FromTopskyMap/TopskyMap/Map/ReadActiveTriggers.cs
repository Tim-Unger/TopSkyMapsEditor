using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopskyMapsEditor;

namespace Parser
{
    public class ReadActiveTriggers
    {
        //TODO multiple, separated with comma
        public static List<Active>? GetActiveTriggers(string content)
        {
            //TODO refactor
            List<Active> activeTriggers = new List<Active>();

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

                                List<string> activeAreas = new();
                                List<string> notActiveAreas = new();

                                activeAreas.Add(areaGroups[1].Value);

                                if (areaGroups[2].Success == true)
                                {
                                    notActiveAreas.Add(areaGroups[2].Value);
                                }

                                active.ActiveAreasList = activeAreas;
                                active.NotActiveAreasList = notActiveAreas;
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

                        //TODO Read Station IDs
                        if (activeType == "ID") { }
                    }

                    activeTriggers.Add(active);
                }
            }

            return activeTriggers;
        }

    }
}
