namespace Pencil.NMeter
{
    using System.Reflection;
    using System.Text.RegularExpressions;
using System.Collections.Generic;

    public class IgnoreFilter
    {
        IgnoreFilterConfiguration configuration;
        Dictionary<string, bool> names = new Dictionary<string, bool>();


        public static IgnoreFilter From(IgnoreFilterConfiguration configuration)
        {
            var filter = new IgnoreFilter() { configuration = configuration };
            foreach(var item in configuration.Names)
                filter.names.Add(item.Value, true);
            return filter;
        }

        public bool Include(AssemblyName name)
        {
            string s = name.Name;
            return !MatchesName(s) && !MatchesPattern(s);
        }

        bool MatchesName(string s) { return names.ContainsKey(s); }

        bool MatchesPattern(string s)
        {
            foreach(var item in configuration.Patterns)
                if(Regex.Match(s, item.Value).Success)
                    return true;
            return false;
        }
    }
}