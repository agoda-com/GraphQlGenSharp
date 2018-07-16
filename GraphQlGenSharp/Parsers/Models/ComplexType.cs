using System;

namespace GraphQlGenSharp.Parsers.Models
{
    public class ComplexType
    {
        private readonly bool _isNullable;

        public ComplexType(string typeName, bool isNullable, bool isList)
        {
            _isNullable = isNullable;
            TypeName = typeName;
            IsList = isList;
        }

        public string TypeName { get; }
        public bool IsList { get; }

        public bool IsNullable()
        {
            return _isNullable;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ComplexType) obj;

            return other.IsList == this.IsList && other._isNullable == this._isNullable &&
                   other.TypeName == this.TypeName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_isNullable, TypeName, IsList);
        }
    }
}
