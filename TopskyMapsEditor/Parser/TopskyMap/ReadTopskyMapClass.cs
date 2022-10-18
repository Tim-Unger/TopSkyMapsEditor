using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using TopskyMapsEditor;
using TopskyMapsEditor.Parser.TopskyMap;

namespace Parser
{
    internal class TopskyMapClass
    {
        public static List<Folder> GetTopskyFolders(string Raw)
        {
            List<Folder> mapFolders = new List<Folder>();
            List<string> folderNames = new List<string>();

            //TODO
            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:)", RegexOptions.Multiline);

            MatchCollection mapMatches = mapRegex.Matches(Raw);

            //Get all the folders
            List<string> folders = new List<string>();
            foreach (Match match in mapMatches)
            {
                GroupCollection groups = match.Groups;
                string RegexValue = groups[0].Value;

                Regex folderRegex = new Regex("FOLDER:(.+)");

                MatchCollection folderMatches = folderRegex.Matches(RegexValue);

                GroupCollection folderGroups = folderMatches[0].Groups;

                if (!folders.Contains(folderGroups[1].Value))
                {
                    folders.Add(folderGroups[1].Value);
                }
            }

            //Add the correct maps to the corret folders
            foreach (var folder in folders)
            {
                Folder addFolder = new Folder();
                List<string> addFolderContent = new List<string>();
                addFolder.FolderName = folder;
                foreach (Match match in mapMatches)
                {
                    GroupCollection groups = match.Groups;
                    string RegexValue = groups[0].Value;

                    Regex folderRegex = new Regex("FOLDER:(.+)");
                    MatchCollection folderMatches = folderRegex.Matches(RegexValue);
                    GroupCollection folderGroups = folderMatches[0].Groups;

                    if (folderGroups[1].Value == folder)
                    {
                        Regex nameRegex = new Regex("MAP:(.+)");
                        MatchCollection nameMatches = nameRegex.Matches(RegexValue);
                        GroupCollection nameGroups = nameMatches[0].Groups;

                        addFolderContent.Add(nameGroups[1].Value);
                    }
                }
                addFolder.FolderContent = addFolderContent;
                mapFolders.Add(addFolder);
            }
            //foreach(var line in lines)
            //{
            //    Regex nameRegex = new Regex("MAP:(.{1,})");

            //    MatchCollection nameMatches = nameRegex.Matches(line);

            //    if(nameMatches.Count == 1)
            //    {
            //        GroupCollection groups = nameMatches[0].Groups;

            //        mapNames.Add(groups[1].Value);
            //    }
            //}

            return mapFolders;
        }

        public static List<string> GetTopskyMapNames(string raw)
        {
            List<string> topskyMaps = new List<string>();

            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:)", RegexOptions.Multiline);

            MatchCollection mapMatches = mapRegex.Matches(raw);

            //Get all the folders
            foreach (Match match in mapMatches)
            {
                string name = null;
                GroupCollection groups = match.Groups;

                string mapNameRegex = groups[0].Value;

                Regex nameRegex = new Regex("MAP:(.{1,})");

                MatchCollection nameMatches = nameRegex.Matches(mapNameRegex);

                if (nameMatches.Count == 1)
                {
                    GroupCollection nameGroups = nameMatches[0].Groups;

                    name = nameGroups[1].Value;
                }

                if (name != null)
                {
                    topskyMaps.Add(name);
                }
            }

            return topskyMaps;
        }

        public static List<TopskyMap> GetTopskyMaps(string raw)
        {
            var StopWatch = Stopwatch.StartNew();

            List<TopskyMap> topskyMaps = new List<TopskyMap>();

            //Gets all the TopskyMaps in the File
            Regex mapRegex = new Regex(@"^MAP:([\s\S]*?)(?=MAP:|\Z)", RegexOptions.Multiline);
            MatchCollection mapMatches = mapRegex.Matches(raw);

            //Gets a single Topsky-Map and parses it
            foreach (Match match in mapMatches)
            {
                TopskyMap topskyMap = new TopskyMap();
                GroupCollection groups = match.Groups;

                topskyMap = ReadTopskyMap(match.ToString());

                topskyMaps.Add(topskyMap);
            }

            //Just for performance testing
            StopWatch.Stop();
            var Time = StopWatch.ElapsedMilliseconds;

            return topskyMaps;
        }

        internal static TopskyMap ReadTopskyMap(string content)
        {
            TopskyMap topskyMap = new TopskyMap();

            //Get the Name
            topskyMap.Name = ReadName.GetName(content);

            //Get the Folder
            topskyMap.Folder = ReadFolder.GetFolder(content);

            //Get the Layer
            topskyMap.Layer = ReadLayer.GetLayer(content);

            //Get the Zoom
            topskyMap.Zoom = ReadZoom.GetZoom(content);

            //Get the Active Types
            topskyMap.ActiveList = ReadActiveTriggers.GetActiveTriggers(content);

            //Get the color
            //TODO Fill color
            topskyMap.Color = ReadColor.GetColor(content);

            //Get the Style
            topskyMap.Style = ReadStyle.GetStyle(content);

            //Get all Lines
            topskyMap.Lines = ReadLines.GetLines(content);

            //Get the Text Properties
            topskyMap.TextProperties = ReadTextProperties.GetTextProperties(content);

            //Get the Texts
            topskyMap.Texts = ReadText.GetText(content);

            return topskyMap;
        }
    }
}
