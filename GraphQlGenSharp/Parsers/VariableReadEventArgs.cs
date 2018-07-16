using System;

namespace GraphQlGenSharp.Parsers
{
    public class VariableReadEventArgs : EventArgs
    {
        public VariableReadEventArgs(string variableName)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }
    }

    public delegate void VariableReadEventHandler(object sender, VariableReadEventArgs a);
}
