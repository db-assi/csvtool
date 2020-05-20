using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADUserMapper_dotnet_console.Utilities
{
    public static class StringOperations
    {
        public static string RemoveSubstring(string s, string sb)
        {
            char substringFirstChar = sb[0];
            char substringLastChar = sb[sb.Length - 1];

            int start = s.IndexOf(substringFirstChar);
            int end = s.IndexOf(substringLastChar) + 1;

            return s.Remove(start, end).Trim();
        }

        public static string RemoveFrom(string s, string from)
        {
            int start = s.IndexOf(from[0]);
            int end = s.Length - start;

            return s.Remove(start, end).Trim();
        }

        public static string RemoveAfter(string s, string after)
        {
            int start = s.IndexOf(s[0]);
            int end = s.IndexOf(after[after.Length -1]) + 1;

            return s.Remove(start, end).Trim();
        }



    }
}
