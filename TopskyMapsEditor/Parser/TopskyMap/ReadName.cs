using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    public class ReadName
    {
        public static string GetName(string content)
        {
            string name = null;
            Regex nameRegex = new Regex("^MAP:(.{1,})", RegexOptions.Multiline);
            MatchCollection nameMatches = nameRegex.Matches(content);
            if (nameMatches.Count == 1)
            {
                GroupCollection groups = nameMatches[0].Groups;
                name = groups[1].Value.TrimEnd('\n', '\r');
                
            }
            
            return name;
        }

        
    }
}
