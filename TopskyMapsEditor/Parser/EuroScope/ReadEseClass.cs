using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using TopskyMapsEditor;

namespace Parser
{
    internal class ReadEseClass
    {
        public static List<Position> ReadEse(string Path)
        {
            List<Position> positions = new();

            StreamReader streamReader = new(Path);

            string File = streamReader.ReadToEnd();
            streamReader.Close();
            string[] lines = File.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            int positionsStartIndex =
                Array.FindIndex(lines, content => content == "[POSITIONS]") + 1;

            string[] positionsarray = lines[positionsStartIndex..];

            foreach (var line in positionsarray)
            {
                if (!line.StartsWith("[") && line != "")
                {
                    Position position = new();

                    Regex positionRegex = new(
                        "(([A-Z]{2,4})((-|_)([A-Z]{1,3}|[0-9]{1,3}))?_(ATIS|DEL|GND|TWR|APP|DEP|CTR|FSS)):(.{1,}):([0-9]{3}.[0-9]{3}):([A-Za-z]{1,}|[0-9]{1,})(?=:)"
                    );

                    MatchCollection positionMatches = positionRegex.Matches(line);

                    if (positionMatches.Count == 1)
                    {
                        GroupCollection groups = positionMatches[0].Groups;

                        position.Callsign = groups[1].Value;
                        position.Name = groups[7].Value;
                        position.Frequency = groups[8].Value;
                        position.SectorIndicator = groups[9].Value;
                    }

                    positions.Add(position);
                }
                else
                {
                    break;
                }
            }
            return positions;
        }

        public static string? CheckIfEseExists(string Path)
        {
            string directory = System.IO.Path.GetDirectoryName(Path);
            string[]? files = Directory.GetFiles(directory, "*.ese", SearchOption.TopDirectoryOnly);

            if (files.Length == 1)
            {
                return files[0];
            }
            else
            {
                return null;
            }
        }
    }
}
