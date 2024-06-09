using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;

namespace ProblemSolving.Templates.Tests.Benchmarks
{
    [MediumRunJob(RuntimeMoniker.Net60)]
    [DisassemblyDiagnoser]
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
        public void GroupGenericSumSegPointUpdateRangeSum() => Fuzz(new GroupGenericSumSegPointUpdateRangeSum());

        [Benchmark]
        public void GenericSumSegPointUpdateRangeSum() => Fuzz(new GenericSumSegPointUpdateRangeSum());

        [Benchmark(Baseline = true)]
        public void SumSegPointUpdateRangeSum() => Fuzz(new SumSegPointUpdateRangeSum());
    }
}
