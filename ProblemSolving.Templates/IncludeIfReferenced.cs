using System;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates
{
    internal sealed class IncludeIfReferenced : Attribute
    {
        public string CallerPath { get; }

        public IncludeIfReferenced([CallerFilePath] string callerPath = default!)
        {
            CallerPath = callerPath;
        }
    }
}
