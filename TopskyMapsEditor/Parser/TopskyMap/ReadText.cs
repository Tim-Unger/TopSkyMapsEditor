using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TopskyMapsEditor.Parser.TopskyMap
{
    public class ReadText
    {
        public static List<Text>? GetText(string content)
        {
            List<Text> returnText = new List<Text>();

            //TODO (fix name only allows letters)
            Regex textRegex = new Regex(
                "TEXT(/(L|C|R)(T|C|B))?:(([A-Za-z]{1,})|(N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|W)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}))|([A-Z]{4}/[0-9]{1,2}(L|C|R))):([A-Za-z]{1,})(:([0-9]{1,4}):([0-9]{1,4}))?",
                RegexOptions.Multiline
            );
            MatchCollection textMatches = textRegex.Matches(content);

            if (textMatches.Count > 0)
            {
                foreach (Match match in textMatches)
                {
                    Text text = new();

                    /*
                    Alignment 2+3
                    Fix Name 5
                    Coordinates N/S 6+7
                    Coordinates E/W 9+10
                    Runway Identifier 11+12
                    Text 13
                    Offset X 15
                    Offset Y 16
                    */
                    GroupCollection textGroups = match.Groups;

                    //Alignment specified
                    if (textGroups[1].Success == true)
                    {
                        string textAlignemnt = textGroups[2].Value + textGroups[3].Value;

                        Alignment alignment = new();
                        switch (textAlignemnt.ToUpper())
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

                        text.TextAlignment = alignment;
                    }

                    //Text is over Fix
                    if (textGroups[5].Success == true)
                    {
                        text.Point = textGroups[5].Value;
                    }
                    //Text is over coordinate
                    if (textGroups[6].Success == true)
                    {
                        text.Latitude = textGroups[6].Value + textGroups[7].Value;
                        text.Longitude = textGroups[8].Value + textGroups[9].Value;
                    }
                    //Text is over Runway
                    //TODO Test
                    if(textGroups[11].Success == true)
                    {
                        text.Point = textGroups[11].Value + textGroups[12].Value;
                    }

                    text.TextContent = textGroups[11].Value;

                    if (textGroups[12].Success == true)
                    {
                        text.OffsetX = int.Parse(textGroups[13].Value);
                        text.OffsetY = int.Parse(textGroups[14].Value);
                    }

                    returnText.Add(text);
                }

                return returnText;
            }
            else
            {
                return null;
            }
        }
    }
}
