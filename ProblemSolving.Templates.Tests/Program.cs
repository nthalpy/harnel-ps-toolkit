using BenchmarkDotNet.Running;

namespace ProblemSolving.Templates.Tests
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .RunAll();
        }
    }
}
