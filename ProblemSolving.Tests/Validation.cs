namespace ProblemSolving.Tests
{
    public abstract class Validation<TInterface, TResult>
    {
        public abstract TResult Fuzz(TInterface f, int randomSeed);
        public abstract void Validate(TResult slow, TResult fast);
    }
}
