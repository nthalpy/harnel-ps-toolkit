using ProblemSolving.Templates.LazySegmentTree.Impl;

namespace ProblemSolving.TestInterfaces.RangeUpdateRangeQuery
{
    public class LazySegRangeUpdateRangeSum : RangeUpdateRangeSumInterface
    {
        private LazySumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new LazySumSeg(size);
        }

        public override long RangeSum(int stIncl, int edExcl) => _seg.RangeQuery(stIncl, edExcl);
        public override void RangeUpdate(int stIncl, int edExcl, long val) => _seg.RangeUpdate(stIncl, edExcl, val);
    }
}
