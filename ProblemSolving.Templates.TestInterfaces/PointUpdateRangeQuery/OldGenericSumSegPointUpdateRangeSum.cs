using ProblemSolving.Templates.Merger;
using ProblemSolving.Templates.SegmentTree;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class OldGenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        [IncludeIfReferenced]
        public sealed class OldGenericSumSeg : OldGenericSeg<long, long>
        {
            public OldGenericSumSeg(int size) : base(size)
            {
            }

            protected override long Merge(long l, long r) => l + r;
            protected override long UpdateElement(long element, long val) => val;
        }

        private OldGenericSumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new OldGenericSumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
