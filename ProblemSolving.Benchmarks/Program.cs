using BenchmarkDotNet.Running;

namespace ProblemSolving.Templates.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Benchmark<>).Assembly).Run(args);
        }
    }
}
