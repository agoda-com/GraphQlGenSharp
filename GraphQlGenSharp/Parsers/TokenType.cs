using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQlGenSharp.Parsers
{
    public enum TokenType
    {
        Space,
        BraceStart,
        BraceEnd,
        NewLine,
        Text,
        Query,
        ObjectName,
        Variable,
        EqualSign,
        Colon,
        BlockStart,
        BlockEnd,
        Block,
        ParamList,
        Comma,
        ParsedParameters
    }
}
