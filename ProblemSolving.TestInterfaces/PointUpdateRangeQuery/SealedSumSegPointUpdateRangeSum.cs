using ProblemSolving.Templates.SegmentTree;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class SealedSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public sealed class SumSeg : AbelianGroupSegTree<long, long, long>
        {
            public SumSeg(int size) : base(size)
            {
            }

            protected override long ApplyDiff(long element, long diff) => element + diff;
            protected override long CreateDiff(long element, long val) => val - element;
            protected override long Identity() => 0;
            protected override long Merge(long l, long r) => l + r;
        }

        private SumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new SumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
