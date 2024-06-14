using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;

namespace ProblemSolving.Templates.Benchmarks.Impl
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [SimpleJob(RuntimeMoniker.Net80)]
    public partial class PointUpdateRangeSumInterfaceBenchmark : Benchmark<PointUpdateRangeSumInterface>
    {
        [Params(1_000_000, 2_000_000, 5_000_000)]
        public int SegTreeSize;

        [Params(1_000_000, 2_000_000, 5_000_000)]
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
        public void OldGroupGenericSumSegPointUpdateRangeSum() => Fuzz(new OldGroupGenericSumSegPointUpdateRangeSum());

        [Benchmark]
        public void GroupGenericSumSegPointUpdateRangeSum() => Fuzz(new GroupGenericSumSegPointUpdateRangeSum());

        [Benchmark]
        public void OldGenericSumSegPointUpdateRangeSum() => Fuzz(new OldGenericSumSegPointUpdateRangeSum());

        [Benchmark]
        public void GenericSumSegPointUpdateRangeSum() => Fuzz(new GenericSumSegPointUpdateRangeSum());

        [Benchmark]
        public void NonAssociativeGenericSumSegPointUpdateRangeSum() => Fuzz(new NonAssociativeGenericSumSegPointUpdateRangeSum());

        [Benchmark(Baseline = true)]
        public void SumSegPointUpdateRangeSum() => Fuzz(new SumSegPointUpdateRangeSum());
    }
}
