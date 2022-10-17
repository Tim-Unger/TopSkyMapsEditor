using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopskyMapsEditor;

namespace Parser
{
    public class ReadLines
    {
        public static List<Line>? GetLines(string content)
        {
            List<Line> returnList = new List<Line>();

            Regex lineRegex = new Regex(
                "LINE:(([A-Z]{2,5}):([A-Z]{2,5})|(((N|S)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|w)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((N|E)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|W)[0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}))|([A-Z]{4}/[0-9]{1,2}(L|C|R)?):([A-Z]{4}/[0-9]{1,2}(L|C|R)?))",
                RegexOptions.Multiline
            );

            MatchCollection lineMatches = lineRegex.Matches(content);

            if (lineMatches.Count > 0)
            {
                List<Line> lines = new();

                foreach (Match match in lineMatches)
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

                returnList = lines;
            }


            return returnList;
        }
    }
}
