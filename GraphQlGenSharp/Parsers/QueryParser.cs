using System;
using System.Collections.Generic;
using System.Linq;
using GraphQlGenSharp.Parsers.Models;

namespace GraphQlGenSharp.Parsers
{
    public class QueryParser
    {
        public event VariableReadEventHandler VariableReadEvent;
        public event ResourceReadEventArgsHandler ResourceReadEvent;

        private readonly QueryLexer _lexer;
        private readonly string[] _typeNames;
        private readonly ParameterParser _parameterParser;

        public QueryParser(IEnumerable<string> typeNames)
        {
            _lexer = new QueryLexer();
            _typeNames = typeNames.ToArray();
            _parameterParser = new ParameterParser(_typeNames);

            _parameterParser.VariableReadEvent += (sender, args) =>
            {
                VariableReadEvent?.Invoke(sender, args);
            };
        }

        public T Read<T>(Resource parent, IEnumerator<Token> e) where T : Resource, new()
        {
            Token token = null;
            var resource = new T();

            if (e.TrySkipUntil(TokenType.ObjectName, out token))
            {
                resource.Name = token.Value.ToString();

                if (ResourceReadEvent != null)
                {
                    ResourceReadEvent(this, new ResourceReadEventArgs(resource));
                }

                if (parent != null)
                {
                    parent.Children.Add(resource);
                }

                if (e.TryRead(out token))
                {
                    if (token.TokenType == TokenType.ParamList)
                    {
                        var parsedParams = ((Token[])token.Value).FirstOrDefault() as ParsedToken<Parameter[]>;
                        if (parsedParams != null)
                        {
                            resource.Parameters.AddRange(parsedParams.ParsedValue);
                        }
                    }

                    if(e.TrySkipUntil(TokenType.BlockStart, out token))
                    {
                        while (e.TryRead(out token))
                        {
                            if (token.TokenType == TokenType.BlockEnd)
                            {
                                break;
                            }

                            if (token.TokenType == TokenType.Text)
                            {
                                var value = token.Value.ToString();
                                resource.Properties.Add(value, null);
                            }
                            else if (token.TokenType == TokenType.ObjectName)
                            {
                                Read<Resource>(resource, e);
                            }
                        }
                    }
                }
            }

            return resource;
        }

        public IEnumerable<Query> Read(string queryText)
        {
            var e = BuildBlocks(BuildParamLists(_lexer.Read(queryText)
                .SkipTokens(TokenType.Space, TokenType.NewLine))).GetEnumerator();

            Token token = null;
            Query query = null;

            while (e.TryRead(out token))
            {
                if (token.TokenType == TokenType.Query)
                {
                    if (query != null)
                    {
                        yield return query;
                    }

                    query = Read<Query>(null, e);
                }
            }

            if (query != null)
            {
                yield return query;
            }
        }

        public IEnumerable<Token> BuildParamLists(IEnumerable<Token> tokens)
        {
            return BuildTokensBetween(tokens, TokenType.BraceStart, TokenType.BraceEnd, TokenType.ParamList, t =>
                {
                    return _parameterParser.Parse(t);
                });
        }

        public IEnumerable<Token> BuildBlocks(IEnumerable<Token> tokens)
        {
            return tokens;
            //return BuildTokensBetween(tokens, TokenType.BlockStart, TokenType.BlockEnd, TokenType.Block);
        }

        private IEnumerable<Token> BuildTokensBetween(IEnumerable<Token> tokens, TokenType startToken, TokenType endToken, TokenType groupType, Func<IEnumerable<Token>, Token> build = null)
        {
            var isStarted = false;
            var internalTokens = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.TokenType == startToken)
                {
                    isStarted = true;
                    continue;
                }

                if (token.TokenType == endToken)
                {
                    if (build != null)
                    {
                        var parsedTokens = build(internalTokens.ToArray());
                        internalTokens.Clear();
                        internalTokens.Add(parsedTokens);
                    }

                    var blockToken = new TokenGroup(internalTokens.ToArray(), groupType);
                    internalTokens.Clear();
                    isStarted = false;
                    yield return blockToken;
                }


                if (isStarted)
                {
                    internalTokens.Add(token);
                }
                else
                {
                    yield return token;
                }
            }

        }
    }
}
