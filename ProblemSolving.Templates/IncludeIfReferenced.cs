using System;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
        AllowMultiple = false,
        Inherited = false)]
    internal sealed class IncludeIfReferenced : Attribute
    {
        public string CallerPath { get; }

        public IncludeIfReferenced([CallerFilePath] string callerPath = default!)
        {
            CallerPath = callerPath;
        }
    }
}
