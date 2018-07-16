using System;
using System.Collections.Generic;
using System.Linq;
using GraphQlGenSharp.Parsers.Models;

namespace GraphQlGenSharp.Parsers
{
    public class ParameterParser
    {
        public event VariableReadEventHandler VariableReadEvent;

        private string[] _typeNames;

        public ParameterParser(string[] typeNames)
        {
            _typeNames = typeNames;
        }

        public ParsedToken<Parameter[]> Parse(IEnumerable<Token> tokens)
        {
            Parameter parameter = null;
            var parameters = new List<Parameter>();

            var e = tokens.GetEnumerator();

            Token token;

            while (e.TryRead(out token))
            {
                if (token.TokenType == TokenType.Comma)
                {
                    continue;
                }

                if (token.TokenType == TokenType.ObjectName || token.TokenType == TokenType.Text)
                {
                    parameter = new Parameter {Name = token.Value.ToString()};

                    if (e.TrySkipUntil(TokenType.Colon, out token) && e.TrySkipUntil(TokenType.ObjectName, out token))
                    {
                        var value = token.Value.ToString();

                        if (value.StartsWith('$'))
                        {
                            parameter.DefaultValue = value;
                            VariableReadEvent?.Invoke(this, new VariableReadEventArgs(parameter.DefaultValue));
                        }
                        else
                        {
                            if (_typeNames != null && !_typeNames.Contains(value))
                            {
                                throw new Exception($"Unknown type: {value}");
                            }

                            parameter.ParameterType = value.ParseTypeNameType();

                            if (parameter.IsVariable && e.TryRead(out token))
                            {
                                if (token.TokenType == TokenType.EqualSign)
                                {
                                    if (e.TryRead(out token))
                                    {
                                        parameter.DefaultValue = token.Value.ToString();
                                    }
                                }
                            }
                        }
                    }

                    parameters.Add(parameter);
                }
            }

            return new ParsedToken<Parameter[]>(parameters.ToArray(), TokenType.ParsedParameters);
        }
    }
}
