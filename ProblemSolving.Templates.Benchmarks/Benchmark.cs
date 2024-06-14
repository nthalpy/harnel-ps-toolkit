namespace ProblemSolving.Templates.Benchmarks
{
    public abstract class Benchmark<TInterface>
    {
        public abstract void Fuzz(TInterface f);
    }
}
