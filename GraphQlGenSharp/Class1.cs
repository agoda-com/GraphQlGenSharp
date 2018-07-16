using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQlGenSharp.Introspection;
using GraphQlGenSharp.Parsers;
using GraphQlGenSharp.Parsers.Models;

namespace GraphQlGenSharp
{
    public class Class1
    {
        private string _graphqlUrl;
        private IEnumerable<string> _queries;

        public Class1(string graphqlUrl, IEnumerable<string> queries)
        {
            _graphqlUrl = graphqlUrl;
            _queries = queries;
        }

        public async Task Generate()
        {
            var r = new MetaRequestClient(_graphqlUrl);

            var typeNames = await r.GetTypeNamesAsync();

            var parser = new QueryParser(typeNames);

            var variables = new List<string>();
            var resources = new List<Resource>();

            parser.VariableReadEvent += (sender, args) =>
            {
                variables.Add(args.VariableName);
            };

            parser.ResourceReadEvent += (sender, args) =>
            {
                if (args.Resource is Query)
                {
                    return;
                }

                resources.Add(args.Resource);
            };

            foreach (var query in _queries)
            {
                foreach (var token in parser.Read(query))
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}
