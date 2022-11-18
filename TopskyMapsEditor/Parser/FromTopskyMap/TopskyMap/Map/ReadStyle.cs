using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TopskyMapsEditor;
using Style = TopskyMapsEditor.Style;

namespace Parser
{
    public class ReadStyle
    {
        public static Style? GetStyle(string content)
        {
            Style? returnStyle = null;

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

                int? width = groups[2].Success ? int.Parse(groups[2].Value) : null;

                style.Width = width;

                returnStyle = style;
            }

            return returnStyle;
        }
    }
}
