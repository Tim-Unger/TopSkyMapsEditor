using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    public class ReadZoom
    {
        public static int? GetZoom(string content)
        {
            int? zoom = null;

            Regex zoomRegex = new Regex("^ZOOM:([0-9]{1,})", RegexOptions.Multiline);
            MatchCollection zoomMatches = zoomRegex.Matches(content);
            if (zoomMatches.Count == 1)
            {
                GroupCollection groups = zoomMatches[0].Groups;
               zoom = int.Parse(groups[1].Value.TrimEnd('\n', '\r'));
            }

            return zoom;
        }
    }
}
