using System;
using GraphQlGenSharp.Introspection;
using GraphQlGenSharp.Parsers;

namespace GraphQlGenSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = @"
query MyQuery($id: Int = 1, $o: String) {
  Property(propertyId: $id) {
    id
    name
    Continent {
      id
      name
    }
    Country {
      id
      Languages {
        languageId
        countryName
      }
    }
    Region {
      id
      name
    }
    State {
      id
      Languages {
        languageId
        stateName
      }
    }
    City {
      id
      Languages {
        languageId
        cityName
      }
    }
    Area {
      id
      name
    }
  }
  Languages {
    languageId
    name
  }
}
";

            var grUrl = "http://bk-qaycsa-1001/v2/graphql";

            var c = new Class1(grUrl, new []{ query });

            c.Generate().Wait();

            Console.ReadLine();
        }
    }
}
