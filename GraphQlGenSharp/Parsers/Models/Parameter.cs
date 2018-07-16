namespace GraphQlGenSharp.Parsers.Models
{
    public class Parameter
    {
        public string Name { get; set; }
        public object ParameterType { get; set; }
        public string DefaultValue { get; set; }

        public bool IsNullable
        {
            get { return false; }
        }

        public bool IsScalarType
        {
            get { return false; }
        }

        public bool IsList
        {
            get { return false; }
        }

        public bool IsVariable
        {
            get { return Name.StartsWith("$"); }
        }
    }
}
