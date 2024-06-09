namespace ProblemSolving.Templates.Tests
{
    public abstract class Benchmark<TInterface>
    {
        public abstract void Fuzz(TInterface f);
    }
}
