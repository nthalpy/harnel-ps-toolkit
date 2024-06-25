using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using ProblemSolving.TestInterfaces.PointUpdateRangeQuery.Fast;
using System;

namespace ProblemSolving.Templates.Benchmarks.Impl
{
    [LongRunJob(RuntimeMoniker.Net60)]
    [LongRunJob(RuntimeMoniker.Net70)]
    [LongRunJob(RuntimeMoniker.Net80)]
    public partial class PointUpdateRangeSumInterfaceBenchmark : Benchmark<PointUpdateRangeSumInterface>
    {
        [Params(1_000_000/*, 2_000_000, 5_000_000*/)]
        public int SegTreeSize;

        [Params(1_000_000/*, 2_000_000, 5_000_000*/)]
        public int QueryCount;

        [IterationSetup]
        public void Setup()
        {
        }

        public override void Fuzz(PointUpdateRangeSumInterface f)
        {
            var rd = new Random();
            f.Fuzz(rd, SegTreeSize, QueryCount);
        }

        [Benchmark]
        public void ACLStyleSumSegPointUpdateRangeSum() => Fuzz(new ACLStyleSumSegPointUpdateRangeSum());

        //[Benchmark]
        //public void ACLSumSegPointUpdateRangeSum() => Fuzz(new ACLSumSegPointUpdateRangeSum());

        //[Benchmark]
        //public void NonGenericSumSegPointUpdateRangeSum() => Fuzz(new NonGenericSumSegPointUpdateRangeSum());

        //[Benchmark]
        //public void NonSealedSumSegPointUpdateRangeSum() => Fuzz(new NonSealedSumSegPointUpdateRangeSum());

        [Benchmark(Baseline = true)]
        public void SealedSumSegPointUpdateRangeSum() => Fuzz(new SealedSumSegPointUpdateRangeSum());
    }
}
