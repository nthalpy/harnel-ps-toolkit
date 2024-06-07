using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProblemSolving.Templates.SegmentTree;
using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.Benchmark.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.Net80)]
    [DisassemblyDiagnoser]
    public class MaxSegBenchmark
    {
        private enum OperationType
        {
            PointUpdate,
            RangeQuery,
        }

        [Params(1_000_000, 2_000_000, 5_000_000)]
        public int SegTreeSize;

        [Params(1_000_000, 2_000_000, 5_000_000)]
        public int QueryCount;

        private long[] _initialValues = default!;
        private (OperationType type, int arg0, int arg1)[] _queries = default!;

        [IterationSetup]
        public void Setup()
        {
            var rd = new Random();

            _initialValues = new long[SegTreeSize];
            for (var idx = 0; idx < _initialValues.Length; idx++)
                _initialValues[idx] = rd.Next(-1000, 1000);

            _queries = new (OperationType type, int arg0, int arg1)[QueryCount];
            for (var idx = 0; idx < QueryCount; idx++)
            {
                var type = (OperationType)(rd.Next() % 2);

                if (type == OperationType.PointUpdate)
                {
                    var arg0 = rd.Next(0, SegTreeSize);
                    var arg1 = rd.Next(-1000, 1000);

                    _queries[idx] = (type, arg0, arg1);
                }
                else
                {
                    var arg0 = rd.Next(0, SegTreeSize);
                    var arg1 = rd.Next(1 + arg0, 1 + SegTreeSize);

                    _queries[idx] = (type, arg0, arg1);
                }
            }
        }

        [Benchmark]
        public List<long> GenericMaxSeg()
        {
            var result = new List<long>();
            var seg = new MaxSeg(SegTreeSize);
            seg.Init(_initialValues);

            foreach (var (type, arg0, arg1) in _queries)
            {
                if (type == OperationType.PointUpdate)
                    seg.Update(arg0, arg1);
                else if (type == OperationType.RangeQuery)
                    result.Add(seg.Range(arg0, arg1));
                else
                    throw new InvalidOperationException();
            }

            return result;
        }

        [Benchmark(Baseline = true)]
        public List<long> NonGenericMaxSeg()
        {
            var result = new List<long>();
            var seg = new MaxSegV1(SegTreeSize);
            seg.Init(_initialValues);

            foreach (var (type, arg0, arg1) in _queries)
            {
                if (type == OperationType.PointUpdate)
                    seg.Update(arg0, arg1);
                else if (type == OperationType.RangeQuery)
                    result.Add(seg.Range(arg0, arg1));
                else
                    throw new InvalidOperationException();
            }

            return result;
        }
    }
}
