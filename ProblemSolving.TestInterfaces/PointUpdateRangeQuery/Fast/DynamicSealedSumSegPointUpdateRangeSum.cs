using ProblemSolving.Templates.SegmentTree;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;

namespace ProblemSolving.TestInterfaces.PointUpdateRangeQuery.Fast
{
    public class DynamicSealedSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public sealed class SumSeg : DynamicAbelianGroupSegTree<long, long, long>
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

        public override void PointUpdate(int index, long val) => _seg.UpdateValue(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
