using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopskyMapsEditor;

namespace Parser
{
    public class ReadColor
    {
        public static string? GetColor(string content)
        {
            string? color = null;

            Regex colorRegex = new Regex("COLOR:(.{1,})", RegexOptions.Multiline);
            MatchCollection colorMatches = colorRegex.Matches(content);
            if (colorMatches.Count == 1)
            {
                GroupCollection colorGroups = colorMatches[0].Groups;

                color = colorGroups[1].Value;
            }

            return color;
        }
    }
}
