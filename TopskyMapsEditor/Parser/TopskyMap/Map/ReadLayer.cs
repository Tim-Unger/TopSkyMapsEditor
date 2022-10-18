using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TopskyMapsEditor.Parser.TopskyMap
{
    public class ReadLayer
    {
        public static int? GetLayer(string content)
        {
            int? layer = null;

            Regex layerRegex = new Regex("^LAYER:([0-9]{1,})", RegexOptions.Multiline);
            MatchCollection layerMatches = layerRegex.Matches(content);
            if (layerMatches.Count == 1)
            {
                GroupCollection groups = layerMatches[0].Groups;
                layer = int.Parse(groups[1].Value.TrimEnd('\n', '\r'));
            }
            else if (layerMatches.Count == 0)
            {
                layer = null;
            }

            return layer;
        }
    }
}
