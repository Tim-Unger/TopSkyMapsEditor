using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopskyMapsEditor;

namespace Parser
{
    internal class TopskyMapClass
    {
        public static List<Folder> TopskyMapNames(string Raw)
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

            ReadTopskyMap("23");

            return mapFolders;
        }

        internal static TopskyMap ReadTopskyMap(string Raw)
        {
            TopskyMap topskyMap = new TopskyMap();

            return topskyMap;
        }
    }
}
