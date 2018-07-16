using System;
using System.Collections.Generic;

namespace GraphQlGenSharp.Parsers
{
    public class NameAliasesAttribute : Attribute
    {
        public NameAliasesAttribute(string name, params string[] aliases)
        {
            var names = new List<string>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                names.Add(name);
            }

            if (aliases != null)
            {
                foreach (var alias in aliases)
                {
                    if (!string.IsNullOrWhiteSpace(alias))
                    {
                        names.Add(alias);
                    }
                }
            }

            Aliases = names.ToArray();
        }

        public string[] Aliases { get; }
    }
}
