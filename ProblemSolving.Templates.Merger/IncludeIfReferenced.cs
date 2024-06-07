using System.Runtime.CompilerServices;

namespace ProblemSolving
{
    /// <summary>
    /// This file is intentionally placed at ProblemSolving namespace
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class IncludeIfReferenced : Attribute
    {
        public string CallerPath { get; }

        public IncludeIfReferenced([CallerFilePath] string callerPath = default!)
        {
            CallerPath = callerPath;
        }
    }
}
