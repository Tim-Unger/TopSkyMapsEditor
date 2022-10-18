using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Color = TopskyMapsEditor.Color;

namespace Parser
{
    public class ReadColors
    {
        public static List<Color> GetColors(string content)
        {
            List<Color> colors = new List<Color>();

            Regex colorRegex = new Regex("COLORDEF:(.{1,}):([0-9]{1,3}):([0-9]{1,3}):([0-9]{1,3})", RegexOptions.Multiline);

            MatchCollection colorMatches = colorRegex.Matches(content);

            if(colorMatches.Count > 0)
            {
                foreach(Match match in colorMatches)
                {
                    Color color = new();

                    /*
                     Name 1
                    red 2
                    green 3
                    blue 4
                    */
                    GroupCollection colorgroups = match.Groups;

                    color.Name = colorgroups[1].Value;

                    color.RedValue= int.Parse(colorgroups[2].Value);
                    color.GreenValue = int.Parse(colorgroups[3].Value);
                    color.BlueValue = int.Parse(colorgroups[4].Value);

                    color.RGBColor = System.Windows.Media.Color.FromRgb(byte.Parse(colorgroups[2].Value), byte.Parse(colorgroups[3].Value), byte.Parse(colorgroups[4].Value));

                    colors.Add(color);
                }

                return colors;

            }
            else
            {
                return null;
            }
        }
    }
}
