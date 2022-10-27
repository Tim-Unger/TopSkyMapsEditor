using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TopskyMapsEditor.Parser.TopskyMap
{
    public class ReadTextProperties
    {
        public static TextProperties GetTextProperties(string content)
        {
            TextProperties textProperties = new();

            //Get Fontsize
            Regex fontSizeRegex = new Regex(
                @"FONTSIZE:(0|(=|-|\+|\*))(:([0-9]{1,2}))?",
                RegexOptions.Multiline
            );
            MatchCollection fontSizeMatches = fontSizeRegex.Matches(content);
            //TODO
            //if(fontSizeMatches.Count > 0)
            //{
            //    foreach(Match match in fontSizeMatches)
            //    {

            //    }
            //}
            if (fontSizeMatches.Count == 1)
            {
                textProperties.FontSize = GetFontSize(fontSizeMatches[0]);
            }

            //Get Fontstyle
            Regex fontStyleRegex = new Regex(
                "FONTSTYLE:(0|([0-9]{1,4}))(:(0|1):(0|1):(0|1))?",
                RegexOptions.Multiline
            );
            MatchCollection fontStyleMatches = fontStyleRegex.Matches(content);
            if (fontStyleMatches.Count == 1) 
            {
                textProperties.FontStyle = GetFontStyle(fontStyleMatches[0]);
            }

            //Get Fontalignment
            Regex fontAlignRegex = new Regex("TEXTALIGN:(L|C|R)(T|C|B)", RegexOptions.Multiline);
            MatchCollection fontAlignMatches = fontAlignRegex.Matches(content);
            if(fontAlignMatches.Count == 1)
            {
                textProperties.TextAlignment = GetFontAlignment(fontAlignMatches[0]);
            }

            return textProperties;
        }

        private static FontSize GetFontSize(Match match)
        {
            FontSize fontSize = new();

            GroupCollection fontSizeGroups = match.Groups;

            string Type = fontSizeGroups[1].Value;
            if (fontSizeGroups[4].Success == true)
            {
                fontSize.Size = int.Parse(fontSizeGroups[4].Value);
            }

            switch (Type)
            {
                case "=":
                    fontSize.Type = FontSizeType.New;
                    break;
                case "-":
                    fontSize.Type = FontSizeType.Reduce;
                    break;
                case "+":
                    fontSize.Type = FontSizeType.Increase;
                    break;
                case "*":
                    fontSize.Type = FontSizeType.Multiply;
                    break;
            }

            return fontSize;
        }

        private static FontStyle GetFontStyle(Match match)
        {
            FontStyle style = new();

            GroupCollection fontStyleGroups = match.Groups;

            style.FontWeigth = int.Parse(fontStyleGroups[1].Value);

            if (fontStyleGroups[4].Value == "1")
            {
                style.IsItalic = true;
            }
            if(fontStyleGroups[5].Value == "1")
            {
                style.IsUnderlined = true;
            }
            if (fontStyleGroups[6].Value == "1")
            {
                style.IsStrikethrough = true;
            }

            return style;
        }

        private static Alignment GetFontAlignment(Match match)
        {
            Alignment alignment = new();

            GroupCollection fontAlignmentGroups = match.Groups;

            string horizontalAlignment = fontAlignmentGroups[0].Value;
            string verticalAlignment = fontAlignmentGroups[1].Value;
            string combinedAlignment = horizontalAlignment + verticalAlignment;

            switch (combinedAlignment)
            {
                case "LT":
                    alignment = Alignment.TopLeft;
                    break;
                case "LC":
                    alignment = Alignment.CenterLeft;
                    break;
                case "LB":
                    alignment = Alignment.BottomLeft;
                    break;

                case "CT":
                    alignment = Alignment.TopCenter;
                    break;
                case "CC":
                    alignment = Alignment.CenterCenter;
                    break;
                case "CB":
                    alignment = Alignment.BottomCenter;
                    break;

                case "RT":
                    alignment = Alignment.TopRight;
                    break;
                case "RC":
                    alignment = Alignment.CenterRight;
                    break;
                case "RB":
                    alignment = Alignment.BottomRight;
                    break;
            }

            return alignment;
        }
    }
}
