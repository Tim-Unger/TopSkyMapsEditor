using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopskyMapsEditor;

namespace Parser
{
    public class ReadFolder : MainWindow
    {
        public static string GetFolder(string content)
        {
            string folder = null;

            Regex folderRegex = new Regex("^FOLDER:(.{1,})", RegexOptions.Multiline);
            MatchCollection folderMatches = folderRegex.Matches(content);
            if (folderMatches.Count == 1)
            {
                GroupCollection groups = folderMatches[0].Groups;
                folder = groups[1].Value.TrimEnd('\n', '\r');
                if (IsNameValid(folder))
                {
                    return folder;
                }
                
                return "Invalid";
                
            }
            
            return "Invalid";
            
        }

        private static bool IsNameValid(string name)
        {
            //Name starts with whitespace, is AUTO or LMAPS or contains Backslash
            if (name.ToCharArray()[0] == ' ' || name.ToUpper() == "AUTO" || name.ToUpper() == "LMAPS" || name.Contains('\\'))
            {
                return false;
            }
           
            return true;
        }
    }
}
