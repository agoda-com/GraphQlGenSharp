using System.Collections.Generic;

namespace GraphQlGenSharp.Parsers.Models
{
    public class Resource
    {
        public Resource()
        {
            Parameters = new List<Parameter>();
            Properties = new Dictionary<string, string>();
            Children = new List<Resource>();
        }

        public string Name { get; set; }
        public List<Parameter> Parameters { get; }
        public Dictionary<string, string> Properties { get; }
        public List<Resource> Children { get; }
    }
}
