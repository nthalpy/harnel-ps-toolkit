using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.Benchmark.Benchmarks
{
    [MediumRunJob(RuntimeMoniker.Net60)]
    [DisassemblyDiagnoser]
    public class SumSegBenchmark
    {
        [Params(1_000_000, 2_000_000, 5_000_000)]
        public int SegTreeSize;

        [Params(1_000_000, 2_000_000, 5_000_000)]
        public int QueryCount;

        [IterationSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public List<long> GroupGenericSumSegPointUpdateRangeSum()
        {
            var rd = new Random();
            return new GroupGenericSumSegPointUpdateRangeSum().Fuzz(rd, SegTreeSize, QueryCount);
        }

        [Benchmark]
        public List<long> GenericSumSegPointUpdateRangeSum()
        {
            var rd = new Random();
            return new GenericSumSegPointUpdateRangeSum().Fuzz(rd, SegTreeSize, QueryCount);
        }

        [Benchmark(Baseline = true)]
        public List<long> SumSegPointUpdateRangeSum()
        {
            var rd = new Random();
            return new SumSegPointUpdateRangeSum().Fuzz(rd, SegTreeSize, QueryCount);
        }
    }
}
