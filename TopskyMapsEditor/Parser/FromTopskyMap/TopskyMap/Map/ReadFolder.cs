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
                else
                {
                    return "Invalid";
                }
            }
            else
            {
                return "Invalid";
            }
        }

        private static bool IsNameValid(string name)
        {
            //Name starts with whitespace
            if (name.ToCharArray()[0] == ' ')
            {
                return false;
            }
            //Name is AUTO or LMAPS
            else if (name.ToUpper() == "AUTO" || name.ToUpper() == "LMAPS")
            {
                return false;
            }
            //Name contains backslash
            else if (name.Contains('\\'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        
    }
}
