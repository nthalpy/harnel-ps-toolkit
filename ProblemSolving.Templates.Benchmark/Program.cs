using BenchmarkDotNet.Running;

namespace ProblemSolving.Templates.Benchmark
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
