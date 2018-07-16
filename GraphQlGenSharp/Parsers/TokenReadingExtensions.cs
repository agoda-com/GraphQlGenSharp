using System.Collections.Generic;
using System.Linq;

namespace GraphQlGenSharp.Parsers
{
    public static class TokenReadingExtensions
    {
        public static bool TryRead<T>(this IEnumerator<T> enumerator, out T value)
        {
            value = default(T);

            if (enumerator.MoveNext())
            {
                value = enumerator.Current;
                return true;
            }

            return false;
        }

        public static IEnumerable<Token> ReadUntil(
            this IEnumerator<Token> enumerator, TokenType tokenType)
        {
            Token token;
            while (enumerator.TryRead(out token))
            {
                if(token != null && token.TokenType != tokenType)
                {
                    yield return token;
                }
            }
        }

        public static bool TrySkipUntil(
            this IEnumerator<Token> enumerator, TokenType tokenType, out Token token)
        {
            token = null;

            if (enumerator.Current != null && enumerator.Current.TokenType == tokenType)
            {
                token = enumerator.Current;
                return true;
            }

            while (enumerator.TryRead(out token))
            {
                if (token.TokenType == tokenType)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<Token> SkipTokens(this IEnumerable<Token> tokens, params TokenType[] tokenTypes)
        {
            if (tokenTypes == null || !tokenTypes.Any())
            {
                return tokens;
            }

            return tokens.Where(t => !tokenTypes.Contains(t.TokenType));
        }
    }
}
