using ProblemSolving.Templates.SegmentTree;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class GroupGenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public struct Op : IAbelianGroupSegOp<long, long, long>
        {
            public long Identity() => 0;
            public long ApplyDiff(long element, long diff) => element + diff;
            public long CreateDiff(long element, long val) => val - element;
            public long Merge(long l, long r) => l + r;
        }

        private AbelianGroupSegTree<long, long, long, Op> _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new AbelianGroupSegTree<long, long, long, Op>(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
