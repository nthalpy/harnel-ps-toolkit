using ProblemSolving.Templates.SegmentTree;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class GenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public struct Op : ISemigroupSegOp<long, long>
        {
            public long Merge(long l, long r) => l + r;
            public long UpdateElement(long elem, long update) => update;
        }

        private SemigroupSegTree<long, long, Op> _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new SemigroupSegTree<long, long, Op>(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
