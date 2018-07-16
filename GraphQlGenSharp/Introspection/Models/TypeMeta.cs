using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GraphQlGenSharp.Introspection.Models
{
    public class MetaResult
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("__schema")]
        public Schema Schema { get; set; }
    }

    public class Schema
    {
        public TypeMeta Types { get; set; }
    }

    public class TypeMeta
    {
        public string Name { get; set; }
        public TypeMeta Fields { get; set; }
        public TypeInfo Type { get; set; }
    }

    public class TypeInfo
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public string OfType { get; set; }
    }
}
