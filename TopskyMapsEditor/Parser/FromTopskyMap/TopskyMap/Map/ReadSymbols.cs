using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TopskyMapsEditor.Parser.TopskyMap
{
    public class ReadSymbols
    {
        public static List<Symbol>? GetSymbols(string content)
        {
            List<Symbol> returnList = new List<Symbol>();

            Regex symbolRegex = new Regex("SYMBOL(/(L|C|R)(T|C|B))?:([A-Za-z]{1,}):(([A-Za-z]{1,})|(N|S)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}):((E|W)([0-9]{3}.[0-9]{2}.[0-9]{2}.[0-9]{3}))|([A-Z]{4}/[0-9]{1,2}(L|C|R))):([A-Za-z]{1,})(:([0-9]{1,4}):([0-9]{1,4}))?", RegexOptions.Multiline);

            MatchCollection symbolmatches = symbolRegex.Matches(content);
            if(symbolmatches.Count > 0)
            {
                foreach (Match match in symbolmatches)
                {
                    Symbol symbol = new();

                    /*
                    Alignment 2 + 3
                    Symbol Name 4
                    Fix 5
                    Coordinates N/ S 7 + 8
                    Coordinates E/ W 10 + 11
                    Runway 12 + 13
                    Label 14
                    Offset X 16
                    Offset Y 17
                    */
                    GroupCollection symbolGroups = match.Groups;

                    //Alignment is defined
                    if (symbolGroups[1].Success == true)
                    {
                        Alignment alignment = new();

                        string textAlignemnt = symbolGroups[2].Value + symbolGroups[3].Value;
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
                        symbol.SymbolAlignment = alignment;
                    }

                    symbol.SymbolName = symbolGroups[4].Value;

                    //Symbol is over Fix
                    if (symbolGroups[6].Success == true)
                    {
                        symbol.Point = symbolGroups[6].Value;
                    }
                    //Symbol is over coordinate
                    if (symbolGroups[7].Success == true)
                    {
                        symbol.Latitude = symbolGroups[7].Value + symbolGroups[8].Value;
                        symbol.Longitude = symbolGroups[10].Value + symbolGroups[11].Value;
                    }
                    //Symbol is over runway
                    if (symbolGroups[12].Success == true)
                    {
                        symbol.Point = symbolGroups[12].Value + symbolGroups[13].Value;
                    }

                    symbol.Label = symbolGroups[14].Value;

                    if (symbolGroups[15].Success == true)
                    {
                        symbol.OffsetX = int.Parse(symbolGroups[16].Value);
                        symbol.OffsetY = int.Parse(symbolGroups[17].Value);
                    }

                    returnList.Add(symbol);
                }

                return returnList;
            }

            return null;

        }
    }
}
