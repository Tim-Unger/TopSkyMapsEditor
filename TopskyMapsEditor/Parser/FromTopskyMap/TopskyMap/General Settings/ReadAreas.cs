using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    public class ReadAreas
    {
        public static List<string>? GetAreas(string mapsPath)
        {
            var topskyDirectory = Path.GetDirectoryName(mapsPath);

            //TODO fix exception here
            var topskyFiles = Directory.GetFiles(topskyDirectory, "*.txt", SearchOption.AllDirectories);

            foreach(var file in topskyFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                if(fileName == "TopSkyAreas")
                {
                    StreamReader streamReader = new(file);
                    string rawText = streamReader.ReadToEnd();
                    streamReader.Close();

                    List<string> areas = ParseAreas(rawText);
                    return areas;
                }
            }
            return null;
        }

        private static List<string> ParseAreas (string rawText)
        {
            List<string> areas = new();

            Regex areaRegex = new("AREA:.{1,2}:(.{1,})", RegexOptions.Multiline);

            MatchCollection areaMatches = areaRegex.Matches(rawText);

            if(areaMatches.Count > 0)
            {
                foreach(Match match in areaMatches.Cast<Match>())
                {
                    areas.Add(match.Groups[1].Value);
                }
            }

            return areas;
        }
    }
}
