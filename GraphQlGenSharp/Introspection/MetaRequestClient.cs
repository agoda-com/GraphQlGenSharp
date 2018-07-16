using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQlGenSharp.Introspection.Models;
using Newtonsoft.Json;

namespace GraphQlGenSharp.Introspection
{
    public class MetaRequestClient
    {
        private const string _contentType = "application/json"; // "application/graphql";

        private const string _getTypeNames = @"
{
  __schema {
    types {
      name
      fields {
        name
        type {
          name
          kind
          ofType {
            name
            kind
          }
        }
      }
    }
  }
}
";

        private IEnumerable<dynamic> _types = null;

        private static readonly HttpClient _client = new HttpClient();

        private readonly string _graphQlUrl;

        public MetaRequestClient(string graphQlUrl)
        {
            _graphQlUrl = graphQlUrl;
        }

        public async Task<IEnumerable<TypeMeta>> GetTypeNamesAsync()
        {
            var json = JsonConvert.SerializeObject(new
            {
                query = _getTypeNames
            });

            var response = await _client.PostAsync(
                _graphQlUrl,
                new StringContent(json, Encoding.UTF8, _contentType));

            var responseString = await response.Content.ReadAsStringAsync();


            var meta = JsonConvert.DeserializeObject<MetaResult>(responseString);
            return meta.Data.Schema.Types;
        }

        
    }
}
