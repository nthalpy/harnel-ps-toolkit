using ProblemSolving.Templates.SegmentTree;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class SumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        private SumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new SumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
