using System;
using System.Collections.Generic;

namespace GraphQlGenSharp.Parsers.Models
{
    public static class TypeExtensions
    {

        private static Dictionary<string, ScalarType> NameToTypeMapping;

        static TypeExtensions()
        {
            var ts = typeof(ScalarType);

            NameToTypeMapping = new Dictionary<string, ScalarType>();

            foreach (var name in Enum.GetNames(ts))
            {
                var e = (ScalarType)Enum.Parse(ts, name);

                var memInfo = ts.GetMember(name);
                var attributes = memInfo[0].GetCustomAttributes(typeof(NameAliasesAttribute), false);
                var aliases = ((NameAliasesAttribute)attributes[0]).Aliases;

                foreach (var alias in aliases)
                {
                    NameToTypeMapping.Add(alias, e);
                }
            }
        }


        public static bool IsNullable(this ScalarType scalarType)
        {
            return Enum.GetName(typeof(ScalarType), scalarType).StartsWith("Nullable");
        }

        public static object ParseTypeNameType(this string value)
        {
            ScalarType? type = null;

            if (value.TryParseTypeNameToScalarType(out type))
            {
                return type.Value;
            }

            var isList = value.StartsWith('[') && value.EndsWith(']');

            if (isList)
            {
                value = value.TrimEnd(']').TrimStart('[');
            }

            var isNullable = value.EndsWith('!');

            if (isNullable)
            {
                value = value.TrimEnd('!');
            }

            return new ComplexType(value, isNullable, isList);
        }

        public static bool TryParseTypeNameToScalarType(this string value, out ScalarType? type)
        {
            type = null;

            ScalarType scalarType;

            if (NameToTypeMapping.TryGetValue(value, out scalarType))
            {
                type = scalarType;
                return true;
            }

            return false;
        }
    }
}
