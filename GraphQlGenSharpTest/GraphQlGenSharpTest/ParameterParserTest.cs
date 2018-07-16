using System;
using System.Collections.Generic;
using System.Linq;
using GraphQlGenSharp.Parsers;
using GraphQlGenSharp.Parsers.Models;
using NUnit.Framework;

namespace GraphQlGenSharpTest
{
    [TestFixture]
    public class ParameterParserTest
    {
        [Test, TestCaseSource(nameof(GetParserTestCases))]
        public void ParsingTest(string paramText, Token[] tokens, ParsedToken<Parameter[]> expectedToken)
        {
            var parser = new ParameterParser(null);
            var parsedToken = parser.Parse(tokens);

            Assert.IsNotNull(parsedToken);
            Assert.AreEqual(parsedToken.TokenType, expectedToken.TokenType);
            Assert.AreEqual(parsedToken.ParsedValue.Length, expectedToken.ParsedValue.Length);
            for (int i = 0; i < parsedToken.ParsedValue.Length; i++)
            {
                var value = parsedToken.ParsedValue[i];
                var expectedValue = expectedToken.ParsedValue[i];
                Assert.AreEqual(value.Name, expectedValue.Name);
                Assert.AreEqual(value.ParameterType, expectedValue.ParameterType);
                Assert.AreEqual(value.DefaultValue, expectedValue.DefaultValue);
            }
        }

        public static IEnumerable<object[]> GetParserTestCases()
        {
            var lexer = new QueryLexer();

            ParsedToken<Parameter[]> CreateToken(params Parameter[] parameters)
            {
                return new ParsedToken<Parameter[]>(parameters, TokenType.ParsedParameters);
            }

            object[] GetTestCase(string paramText, ParsedToken<Parameter[]> expectedToken)
            {
                var tokens = lexer.Read(paramText).SkipTokens(TokenType.BraceStart, TokenType.BraceEnd, TokenType.NewLine, TokenType.Space);

                return new object[] { paramText, tokens.ToArray(), expectedToken};
            }

            yield return GetTestCase("($id: Int = 1, $o: String)",
                CreateToken(
                    new Parameter
                    {
                        Name = "$id",
                        DefaultValue = "1",
                        ParameterType = ScalarType.IntType
                    },
                    new Parameter
                    {
                        Name = "$o",
                        ParameterType = ScalarType.StringType
                    }));

            yield return GetTestCase("($test: String)",
                CreateToken(
                    new Parameter
                    {
                        Name = "$test",
                        ParameterType = ScalarType.StringType
                    }));

            yield return GetTestCase("($test1: String, $test2: Object, $test3: Int = 22)",
                CreateToken(
                    new Parameter
                    {
                        Name = "$test1",
                        ParameterType = ScalarType.StringType
                    },
                    new Parameter
                    {
                        Name = "$test2",
                        ParameterType = new ComplexType("Object", false, false)
                    },
                    new Parameter
                    {
                        Name = "$test3",
                        ParameterType = ScalarType.IntType,
                        DefaultValue = "22"
                    }));

            yield return GetTestCase("($test1: String = \"Jeno\", $test2: Object = null, $test3: Int = 22)",
                CreateToken(
                    new Parameter
                    {
                        Name = "$test1",
                        ParameterType = ScalarType.StringType,
                        DefaultValue = "\"Jeno\""
                    },
                    new Parameter
                    {
                        Name = "$test2",
                        ParameterType = new ComplexType("Object", false, false),
                        DefaultValue = "null"
                    },
                    new Parameter
                    {
                        Name = "$test3",
                        ParameterType = ScalarType.IntType,
                        DefaultValue = "22"
                    }));

            yield return GetTestCase("(propertyId: $id)",
                CreateToken(
                    new Parameter
                    {
                        Name = "propertyId",
                        DefaultValue = "$id"
                    }));
        }
    }
}
