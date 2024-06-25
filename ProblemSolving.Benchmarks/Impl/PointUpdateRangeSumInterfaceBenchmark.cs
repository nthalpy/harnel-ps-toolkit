using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;

namespace ProblemSolving.Templates.Benchmarks.Impl
{
    [VeryLongRunJob(RuntimeMoniker.Net60)]
    [VeryLongRunJob(RuntimeMoniker.Net70)]
    [VeryLongRunJob(RuntimeMoniker.Net80)]
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

        [Benchmark()]
        public void NonGenericSumSegPointUpdateRangeSum() => Fuzz(new NonGenericSumSegPointUpdateRangeSum());

        [Benchmark(Baseline = true)]
        public void SealedSumSegPointUpdateRangeSum() => Fuzz(new SealedSumSegPointUpdateRangeSum());
    }
}
