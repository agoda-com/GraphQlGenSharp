using System.Collections.Generic;

namespace GraphQlGenSharp.Parsers
{
    public class Token
    {
        public Token(object value, TokenType tokenType)
        {
            Value = value;
            TokenType = tokenType;
        }

        public object Value { get; set; }
        public TokenType TokenType { get; set; }

        public override string ToString()
        {
            return $"'{Value}',{TokenType}";
        }
    }

    public class TokenGroup : Token
    {
        public TokenGroup(IEnumerable<Token> values, TokenType tokenType) : base(values, tokenType)
        {
            Values = Values;
        }

        public IEnumerable<Token> Values { get; }
    }

    public class ParsedToken<T> : Token
    {
        public ParsedToken(T value, TokenType tokenType) : base(value, tokenType)
        {
            ParsedValue = value;
        }

        public T ParsedValue { get; }
    }
}
