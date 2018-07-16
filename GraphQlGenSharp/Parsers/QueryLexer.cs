using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQlGenSharp.Parsers
{
    public class QueryLexer
    {
        public IEnumerable<Token> Read(string queryText)
        {
            var textBuilder = new StringBuilder();

            bool TryEmitTextToken(out Token token)
            {
                token = null;
                if (textBuilder.Length <= 0) return false;
                token = GetTextToken(textBuilder.ToString());
                textBuilder.Clear();
                return true;
            }

            foreach (var currentChar in queryText.ToCharArray().Select(c => c.ToString()))
            {
                Token text = null;
                switch (currentChar)
                {
                    case " ":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token(currentChar, TokenType.Space);
                        break;
                    case "\t":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token("  ", TokenType.Space);
                        break;
                    case "{":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token("{", TokenType.BlockStart);
                        break;
                    case "}":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token("}", TokenType.BlockEnd);
                        break;
                    case "(":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token("(", TokenType.BraceStart);
                        break;
                    case ")":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token(")", TokenType.BraceEnd);
                        break;
                    case "\n":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token(Environment.NewLine, TokenType.NewLine);
                        break;
                    case "=":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token("=", TokenType.EqualSign);
                        break;
                    case ",":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token(",", TokenType.Comma);
                        break;
                    case ":":
                        if (TryEmitTextToken(out text))
                        {
                            yield return text;
                        }

                        yield return new Token(":", TokenType.Colon);
                        break;
                    default:
                        if (currentChar != "\r")
                        {
                            textBuilder.Append(currentChar);
                        }

                        break;
                }
            }
        }

        private Token GetTextToken(string value)
        {
            if(value == "query")
            {
                return new Token(value, TokenType.Query);
            }

            if (value[0].ToString() == value[0].ToString().ToUpper())
            {
                return new Token(value, TokenType.ObjectName);
            }

            return new Token(value, TokenType.Text);
        }
    }
}
