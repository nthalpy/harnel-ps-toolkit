using ProblemSolving.Templates.Merger;
using ProblemSolving.Templates.SegmentTree;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class OldGroupGenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        [IncludeIfReferenced]
        public sealed class OldGroupGenericSumSeg : OldGenericGroupSeg<long, long, long>
        {
            public OldGroupGenericSumSeg(int size) : base(size)
            {
            }

            protected override long ApplyDiff(long element, long diff) => element + diff;
            protected override long CreateDiff(long element, long val) => val - element;
            protected override long Merge(long l, long r) => l + r;
        }

        private OldGroupGenericSumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new OldGroupGenericSumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
