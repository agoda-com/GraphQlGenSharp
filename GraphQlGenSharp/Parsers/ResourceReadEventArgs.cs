using System;
using GraphQlGenSharp.Parsers.Models;

namespace GraphQlGenSharp.Parsers
{
    public class ResourceReadEventArgs : EventArgs
    {
        public ResourceReadEventArgs(Resource resource)
        {
            Resource = resource;
        }

        public Resource Resource { get; }
    }

    public delegate void ResourceReadEventArgsHandler(object sender, ResourceReadEventArgs a);
}
